// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.ViewModels.MessageViewModel
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using mvcForum.Core;
using mvcForum.Web.Extensions;
using System;
using System.Collections.Generic;

namespace mvcForum.Web.ViewModels
{
  public class MessageViewModel : ForumViewModelBase
  {
    public MessageViewModel(Post message)
    {
      this.LastRead = false;
      this.Id = message.Id;
      this.Subject = message.Subject;
      this.Body = message.Body;
      this.Author = message.AuthorName;
      this.AuthorId = message.Author.Id;
      ForumUser author = message.Author;
      this.Joined = (DateTimeOffset) author.FirstVisit;
      this.PrettyName = this.Author;
      if (author.UseFullName && !string.IsNullOrWhiteSpace(author.FullName))
        this.PrettyName = author.FullName;
      this.Posted = (DateTimeOffset) message.Posted;
      this.Position = message.Position;
      this.Flag = message.Flag;
      this.TopicId = message.Topic.Id;
      this.TopicLocked = (message.Topic.Flag & TopicFlag.Locked) != TopicFlag.None;
      this.Access = message.Topic.Forum.GetAccess();
      DateTime? edited = message.Edited;
      this.LastEdited = edited.HasValue ? new DateTimeOffset?((DateTimeOffset) edited.GetValueOrDefault()) : new DateTimeOffset?();
      this.ModeratorChanged = message.ModeratorChanged;
      this.DeleteReason = message.DeleteReason;
      this.CustomProperties = (IDictionary<string, string>) message.GetCustomProperties();
    }

    private AccessFlag Access { get; set; }

    public bool IsModerator
    {
      get
      {
        return this.HasAccess(AccessFlag.Moderator);
      }
    }

    public bool CanUpload
    {
      get
      {
        return this.HasAccess(AccessFlag.Upload);
      }
    }

    public bool CanPost
    {
      get
      {
        return this.HasAccess(AccessFlag.Post);
      }
    }

    public bool CanDelete
    {
      get
      {
        return this.HasAccess(AccessFlag.Delete);
      }
    }

    public bool CanEdit
    {
      get
      {
        if (this.HasAccess(AccessFlag.Edit) && this.Authenticated)
          return this.CurrentUser.Id == this.AuthorId;
        return false;
      }
    }

    private bool HasAccess(AccessFlag flag)
    {
      return (this.Access & flag) == flag;
    }

    public int Id { get; private set; }

    public string Subject { get; private set; }

    public string Body { get; private set; }

    public int AuthorId { get; private set; }

    public string Author { get; private set; }

    public DateTimeOffset Joined { get; private set; }

    public DateTimeOffset Posted { get; private set; }

    public int Position { get; private set; }

    public DateTimeOffset? LastEdited { get; private set; }

    public bool ModeratorChanged { get; private set; }

    public string PrettyName { get; private set; }

    public PostFlag Flag { get; private set; }

    public string DeleteReason { get; private set; }

    public bool TopicLocked { get; private set; }

    public int TopicId { get; private set; }

    public TopicViewModel Topic { get; set; }

    public IEnumerable<AttachmentViewModel> Attachments { get; set; }

    public bool LastRead { get; set; }

    public IDictionary<string, string> CustomProperties { get; private set; }
  }
}
