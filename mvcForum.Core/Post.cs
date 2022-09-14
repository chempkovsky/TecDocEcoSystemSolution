// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.Post
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace mvcForum.Core
{
  public class Post : ICustomPropertyHolder
  {
    private DateTime? edited;
    private DateTime posted;

    public Post()
    {
    }

    public Post(ForumUser author, Topic topic, string subject, string body, string ip)
      : this(author, topic, subject, body, ip, (Post) null)
    {
    }

    public Post(
      ForumUser author,
      Topic topic,
      string subject,
      string body,
      string ip,
      Post replyTo)
    {
      this.Topic = topic;
      this.Subject = subject;
      this.Body = body;
      this.IP = ip;
      this.Indent = 0;
      if (replyTo != null)
      {
        this.ReplyToPost = replyTo;
        this.Indent = replyTo.Indent + 1;
      }
      this.Posted = DateTime.UtcNow;
      this.Author = author;
      this.AuthorName = author.Name;
      this.SpamReporters = 0;
      this.SpamScore = 0;
    }

    public int Id { get; set; }

    [Required]
    public int Position { get; set; }

    [Required]
    public int Indent { get; set; }

    [Required]
    [StringLength(200)]
    public string Subject { get; set; }

    [StringLength(2147483647)]
    [Required]
    public string Body { get; set; }

    [Required]
    [StringLength(50)]
    public string IP { get; set; }

    public DateTime? Edited
    {
      get
      {
        return this.edited;
      }
      set
      {
        this.edited = value.Handle();
      }
    }

    public PostFlag Flag
    {
      get
      {
        return (PostFlag) this.FlagValue;
      }
      private set
      {
        this.FlagValue = (int) value;
      }
    }

    [Required]
    public int FlagValue { get; set; }

    [StringLength(500)]
    public string EditReason { get; set; }

    [Required]
    public bool ModeratorChanged { get; set; }

    [StringLength(500)]
    public string DeleteReason { get; set; }

    [Required]
    public int TopicId { get; set; }

    public virtual Topic Topic { get; set; }

    public int? ReplyToPostId { get; set; }

    public virtual Post ReplyToPost { get; set; }

    [Required]
    public int AuthorId { get; set; }

    public virtual ForumUser Author { get; set; }

    [Required]
    [StringLength(256)]
    public string AuthorName { get; set; }

    [Required]
    public DateTime Posted
    {
      get
      {
        return this.posted;
      }
      set
      {
        this.posted = value.Handle();
      }
    }

    [Required]
    public int SpamScore { get; set; }

    [Required]
    public int SpamReporters { get; set; }

    public string CustomProperties { get; set; }

    public XDocument CustomData { get; set; }

    public void ReportSpam(int spamScore)
    {
      ++this.SpamReporters;
      this.SpamScore += this.SpamScore;
    }

    public void SetFlag(PostFlag flag)
    {
      this.Flag = flag;
    }

    public void Update(ForumUser user, string subject, string body)
    {
      int id1 = user.Id;
      int id2 = this.Author.Id;
      this.Update(user, subject, body, string.Empty);
    }

    public void Update(ForumUser user, string subject, string body, string reason)
    {
      if (!(this.Subject != subject) && !(this.Body != body) && string.IsNullOrEmpty(reason))
        return;
      this.Edited = new DateTime?(DateTime.UtcNow);
      if (user.Id != this.Author.Id)
        this.ModeratorChanged = true;
      this.Subject = subject;
      this.Body = body;
      if (string.IsNullOrWhiteSpace(reason))
        return;
      this.EditReason = reason;
    }

    public void Delete(ForumUser user, string reason)
    {
      this.DeleteReason = reason;
      if (this.Author.Id != user.Id)
        this.ModeratorChanged = true;
      this.SetFlag(PostFlag.Deleted);
    }

    public void Undelete(ForumUser user, string reason)
    {
      this.DeleteReason = reason;
      if (this.Author.Id != user.Id)
        this.ModeratorChanged = true;
      this.SetFlag(PostFlag.None);
    }

    public virtual ICollection<Attachment> Attachments { get; set; }
  }
}
