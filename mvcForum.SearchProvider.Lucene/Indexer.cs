// Decompiled with JetBrains decompiler
// Type: mvcForum.SearchProvider.Lucene.Indexer
// Assembly: mvcForum.SearchProvider.Lucene, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 13A5DCA9-908F-4689-9C6D-F584F417B850
// Assembly location: C:\Development\WebCarShop\mvcForum.SearchProvider.Lucene.dll

using ApplicationBoilerplate.Logging;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
using mvcForum.Core;
using mvcForum.Core.Interfaces.AddOns;
using mvcForum.Core.Interfaces.Search;
using mvcForum.Core.Search;
using mvcForum.SearchProvider.Lucene.Indexes;
//using System;
using System.Collections.Generic;
using System.Linq;

namespace mvcForum.SearchProvider.Lucene
{
  public class Indexer : IIndexer, ISearcher, System.IDisposable, ISearchAddOn, IAddOn
  {
    private static object writerLock = new object();
    private readonly string[] specialLuceneCharacters = new string[18]
    {
      "\\",
      "+",
      "-",
      "&&",
      "||",
      "!",
      "(",
      ")",
      "{",
      "}",
      "[",
      "]",
      "^",
      "\"",
      "~",
      "*",
      "?",
      ":"
    };
    private readonly IDirectoryResolver dirResolver;
    private readonly LuceneConfiguration config;
    private readonly ILogger privateLogger;
    private Directory privateDirectory;
    private Analyzer privateAnalyzer;
    private bool disposed;
    private bool disposing;
    private bool initialized;

    private void Log(EventType type, string message)
    {
      if (this.privateLogger == null)
        return;
      this.privateLogger.Log(type, message);
    }

    public Indexer(
      IDirectoryResolver resolver,
      IAddOnConfiguration<Indexer> config,
      ILogger logger)
    {
      this.dirResolver = resolver;
      this.config = (LuceneConfiguration) config;
      this.privateLogger = logger;
    }

    public IEnumerable<SearchResult> Search(string query, IList<int> forums)
    {
      List<SearchResult> source = new List<SearchResult>();
      if (string.IsNullOrWhiteSpace(query))
        return (IEnumerable<SearchResult>) source;
      foreach (string specialLuceneCharacter in this.specialLuceneCharacters)
      {
        if (query.Contains(specialLuceneCharacter))
          query = query.Replace(specialLuceneCharacter, "\\" + specialLuceneCharacter);
      }
      QueryParser queryParser = new QueryParser(Version.LUCENE_29, "Body", this.Analyzer);
      queryParser.DefaultOperator = QueryParser.Operator.AND;
      Query query1 = queryParser.Parse(query);
      if (string.IsNullOrWhiteSpace(query1.ToString()))
        return (IEnumerable<SearchResult>) source;
      string query2 = string.Format("({0}) OR ({1})", (object) query1, (object) query1.ToString().Replace("Body", "Title"));
      Query query3 = queryParser.Parse(query2);
      BooleanQuery booleanQuery1 = new BooleanQuery();
      foreach (int forum in (IEnumerable<int>) forums)
        booleanQuery1.Add((Query) new TermQuery(new Term("ForumId", NumericUtils.IntToPrefixCoded(forum))), Occur.SHOULD);
      BooleanQuery booleanQuery2 = new BooleanQuery();
      booleanQuery2.Add(query3, Occur.MUST);
      booleanQuery2.Add((Query) booleanQuery1, Occur.MUST);
      using (IndexSearcher indexSearcher = new IndexSearcher(this.Directory, true))
      {
        int n = 300;
        TopDocs topDocs = indexSearcher.Search((Query) booleanQuery2, n);
        ScoreDoc[] scoreDocs = topDocs.ScoreDocs;
        int length = scoreDocs.Length;
        int num = 0;
        for (int index = 0; index < length && num < n; ++index)
        {
          Document document = indexSearcher.Doc(scoreDocs[index].Doc);
          source.Add(new SearchResult()
          {
            TopicId = NumericUtils.PrefixCodedToInt(document.Get("TopicId")),
            PostId = NumericUtils.PrefixCodedToInt(document.Get("PostId")),
            Title = document.Get("Title"),
            Score = topDocs.ScoreDocs[index].Score
          });
          ++num;
        }
        indexSearcher.Close();
      }
      return (IEnumerable<SearchResult>) source.OrderByDescending<SearchResult, float>((System.Func<SearchResult, float>) (x => x.Score));
    }

    public void Clear()
    {
      if (!IndexReader.IndexExists(this.Directory))
        return;
      using (IndexWriter writer = this.GetWriter())
      {
        writer.DeleteAll();
        writer.Close();
      }
    }

    public void Remove(Forum forum)
    {
      this.Log(EventType.Debug, string.Format("The forum '{0}' ({1}), is being removed.", (object) forum.Name, (object) forum.Id));
      this.Remove(this.CreateIdSearchQuery(forum));
    }

    public void Remove(Post post)
    {
      using (IndexWriter writer = this.GetWriter())
      {
        this.Remove(post, writer);
        writer.Close();
      }
    }

    private void Remove(Post post, IndexWriter writer)
    {
      this.Log(EventType.Debug, string.Format("The post '{0}' ({1}), is being removed.", (object) post.Subject, (object) post.Id));
      this.Remove(this.CreateIdSearchQuery(post), writer);
    }

    public void Remove(Topic topic)
    {
      this.Log(EventType.Debug, string.Format("The topic '{0}' ({1}), is being removed.", (object) topic.Title, (object) topic.Id));
      this.Remove(this.CreateIdSearchQuery(topic));
    }

    private void Remove(Query query)
    {
      using (IndexWriter writer = this.GetWriter())
      {
        this.Remove(query, writer);
        writer.Close();
      }
    }

    private void Remove(Query query, IndexWriter writer)
    {
      writer.DeleteDocuments(query);
    }

    private void Initialize()
    {
      if (this.initialized)
        return;
      try
      {
        this.privateDirectory = this.dirResolver.GetDirectory();
        this.privateAnalyzer = (Analyzer) new StandardAnalyzer(Version.LUCENE_30);
        this.initialized = true;
      }
      catch
      {
      }
    }

    private IndexWriter GetWriter()
    {
      IndexWriter writer = (IndexWriter) null;
      int num = 0;
      bool flag = false;
      while (writer == null)
      {
        try
        {
          writer = new IndexWriter(this.Directory, this.Analyzer, !IndexReader.IndexExists(this.Directory), IndexWriter.MaxFieldLength.UNLIMITED);
        }
        catch (LockObtainFailedException ex)
        {
          if (num > 1)
            throw ex;
          flag = true;
        }
        try
        {
          if (flag)
          {
            flag = false;
            this.Directory.ClearLock("write.lock");
            if (IndexWriter.IsLocked(this.Directory))
              IndexWriter.Unlock(this.Directory);
          }
        }
        catch (LockObtainFailedException ex)
        {
        }
        ++num;
      }
      writer.SetMergePolicy((MergePolicy) new LogDocMergePolicy(writer));
      writer.MergeFactor = 5;
      return writer;
    }

    private Document CreateDocument(Post post)
    {
      Document document = new Document();
      Field field1 = new Field("PostId", NumericUtils.IntToPrefixCoded(post.Id), Field.Store.YES, Field.Index.NOT_ANALYZED, Field.TermVector.NO);
      Field field2 = new Field("TopicId", NumericUtils.IntToPrefixCoded(post.Topic.Id), Field.Store.YES, Field.Index.NOT_ANALYZED, Field.TermVector.NO);
      Field field3 = new Field("ForumId", NumericUtils.IntToPrefixCoded(post.Topic.Forum.Id), Field.Store.YES, Field.Index.NOT_ANALYZED, Field.TermVector.NO);
      Field field4 = new Field("Title", post.Subject, Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.YES);
      field4.Boost = (float) this.config.TitleWeight / 100f;
      Field field5 = new Field("Body", post.Body, Field.Store.NO, Field.Index.ANALYZED, Field.TermVector.YES);
      Field field6 = new Field("Posted", DateTools.DateToString(post.Posted, DateTools.Resolution.MINUTE), Field.Store.NO, Field.Index.ANALYZED, Field.TermVector.NO);
      Field field7 = new Field("Author", post.AuthorName, Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.NO);
      document.Add((IFieldable) field1);
      document.Add((IFieldable) field2);
      document.Add((IFieldable) field3);
      document.Add((IFieldable) field4);
      document.Add((IFieldable) field5);
      document.Add((IFieldable) field6);
      document.Add((IFieldable) field7);
      if (post.Position == 0)
        document.Boost = !post.Topic.Announcement ? (!post.Topic.Sticky ? (float) this.config.TopicWeight / 100f : (float) this.config.StickyWeight / 100f) : (float) this.config.AnnouncementWeight / 100f;
      return document;
    }

    public void Index(Post post)
    {
      this.AddDocuments((IEnumerable<Post>) new Post[1]
      {
        post
      });
    }

    public void BulkIndex(IEnumerable<Post> posts)
    {
      this.AddDocuments(posts);
    }

    public void Index(Topic topic)
    {
      if (topic.Flag == TopicFlag.Moved)
        return;
      this.AddDocuments((IEnumerable<Post>) topic.Posts);
    }

    private Query CreateIdSearchQuery(Post post)
    {
      return (Query) new TermQuery(new Term("PostId", NumericUtils.IntToPrefixCoded(post.Id)));
    }

    private Query CreateIdSearchQuery(Topic topic)
    {
      return (Query) new TermQuery(new Term("TopicId", NumericUtils.IntToPrefixCoded(topic.Id)));
    }

    private Query CreateIdSearchQuery(Forum forum)
    {
      return (Query) new TermQuery(new Term("ForumId", NumericUtils.IntToPrefixCoded(forum.Id)));
    }

    private void AddDocuments(IEnumerable<Post> posts)
    {
      this.Log(EventType.Debug, "Time to index one or more posts.");
      using (IndexWriter writer = this.GetWriter())
      {
        foreach (Post post in posts)
        {
          this.Remove(post, writer);
          try
          {
            writer.AddDocument(this.CreateDocument(post));
          }
          catch
          {
          }
        }
        if (true)
          writer.Optimize();
        writer.Close();
      }
    }

    private Analyzer Analyzer
    {
      get
      {
        if (!this.disposing)
          this.Initialize();
        return this.privateAnalyzer;
      }
    }

    private Directory Directory
    {
      get
      {
        if (!this.disposing)
          this.Initialize();
        return this.privateDirectory;
      }
    }

    ~Indexer()
    {
      this.Dispose();
    }

    public void Dispose()
    {
      lock (Indexer.writerLock)
      {
        if (!this.disposed)
        {
          this.disposing = true;
          Directory directory = this.Directory;
          if (directory != null)
          {
            try
            {
              directory.Close();
            }
            catch (System.ObjectDisposedException ex)
            {
            }
          }
          this.disposed = true;
        }
      }
            System.GC.SuppressFinalize((object) this);
    }

    private static class FieldNames
    {
      public const string PostId = "PostId";
      public const string TopicId = "TopicId";
      public const string ForumId = "ForumId";
      public const string Title = "Title";
      public const string Body = "Body";
      public const string Posted = "Posted";
      public const string Author = "Author";
    }
  }
}
