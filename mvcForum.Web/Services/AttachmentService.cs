// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.Services.AttachmentService
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using ApplicationBoilerplate.DataProvider;
using ApplicationBoilerplate.Logging;
using mvcForum.Core;
using mvcForum.Core.Abstractions.Interfaces;
using mvcForum.Core.Interfaces;
using mvcForum.Core.Interfaces.Services;
using mvcForum.Core.Specifications;
using System;
using System.IO;
using System.Linq;

namespace mvcForum.Web.Services
{
  public class AttachmentService : IAttachmentService
  {
    private readonly IAttachmentStorage storage;
    private readonly IRepository<Attachment> attachmentRepo;
    private readonly IContext context;
    private readonly IConfiguration configuration;
    private readonly IForumAccessService forumAccess;
    private readonly ILogger log;

    public AttachmentService(
      IAttachmentStorage storage,
      IContext context,
      IConfiguration configuration,
      IForumAccessService fas,
      ILogger log)
    {
      this.storage = storage;
      this.context = context;
      this.attachmentRepo = this.context.GetRepository<Attachment>();
      this.configuration = configuration;
      this.forumAccess = fas;
      this.log = log;
    }

    public AttachStatusCode AttachFile(
      ForumUser author,
      Topic topic,
      string filename,
      string contentType,
      int length,
      Stream stream)
    {
      return this.AttachFile(author, topic.Posts.OrderBy<Post, DateTime>((Func<Post, DateTime>) (p => p.Posted)).First<Post>(), filename, contentType, length, stream);
    }

    public AttachStatusCode AttachFile(
      ForumUser author,
      Post post,
      string filename,
      string contentType,
      int length,
      Stream stream)
    {
      Attachment attachment1 = (Attachment) null;
      try
      {
        if ((this.forumAccess.GetAccessFlag(post.Topic.Forum, author) & AccessFlag.Upload) != AccessFlag.Upload)
          return AttachStatusCode.PermissionsDenied;
        if (this.configuration.MaxFileSize > 0 && this.configuration.MaxFileSize < length)
          return AttachStatusCode.FileTooBig;
        if (this.configuration.MaxAttachmentsSize != 0 && this.configuration.MaxAttachmentsSize < length + this.GetSpaceUsed(author))
          return AttachStatusCode.NotEnoughSpace;
        string str = Path.GetExtension(filename).ToLower();
        if (str.StartsWith("."))
          str = str.Substring(1);
        if (!this.configuration.AllowedExtensions.Contains(str))
          return AttachStatusCode.NotAllowedExtension;
        attachment1 = new Attachment(author, post, "temppath", filename, length);
        this.attachmentRepo.Create(attachment1);
        this.context.SaveChanges();
        string attachment2 = this.storage.CreateAttachment(attachment1, stream, contentType);
        attachment1.Path = attachment2;
        this.context.SaveChanges();
        return AttachStatusCode.Success;
      }
      catch (Exception ex)
      {
        this.log.Log(EventType.Error, "Attaching a file failed", ex);
      }
      if (attachment1 != null && attachment1.Id > 0)
      {
        this.attachmentRepo.Delete(attachment1);
        this.context.SaveChanges();
      }
      return AttachStatusCode.Unknown;
    }

    public int GetSpaceUsed(ForumUser user)
    {
      return this.attachmentRepo.ReadMany((ISpecification<Attachment>) new AttachmentSpecifications.ByAuthor(user)).Sum<Attachment>((Func<Attachment, int>) (a => a.Size));
    }
  }
}
