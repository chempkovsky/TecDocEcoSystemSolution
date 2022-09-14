// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.ViewModels.PostReportViewModel
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using MVCBootstrap.Web.Mvc.Attributes;
using mvcForum.Core;
using System;
using System.ComponentModel.DataAnnotations;

namespace mvcForum.Web.ViewModels
{
  public class PostReportViewModel
  {
    public PostReportViewModel()
    {
    }

    public PostReportViewModel(PostReport pr)
    {
      this.Populate(pr);
    }

    public void Populate(PostReport report)
    {
      this.Id = report.Id;
      this.Reason = report.Reason;
      this.Resolved = report.Resolved;
      this.Feedback = report.Feedback;
      this.Timestamp = (DateTimeOffset) report.Timestamp;
      this.ReporterId = report.ReportedBy.Id;
      this.ReporterName = report.ReportedBy.Name;
      if (report.ResolvedBy != null && report.ResolvedTimestamp.HasValue)
      {
        this.ResolvedById = new int?(report.ResolvedBy.Id);
        this.ResolvedByName = report.ResolvedBy.Name;
        DateTime? resolvedTimestamp = report.ResolvedTimestamp;
        this.ResolvedTimestamp = resolvedTimestamp.HasValue ? new DateTimeOffset?((DateTimeOffset) resolvedTimestamp.GetValueOrDefault()) : new DateTimeOffset?();
      }
      this.IsTopic = false;
      if (report.Post.Position == 0)
      {
        this.IsTopic = true;
        this.Title = report.Post.Topic.Title;
        this.TopicFlag = report.Post.Topic.Flag;
        this.TopicType = report.Post.Topic.Type;
        this.TopicId = report.Post.Topic.Id;
        this.Author = report.Post.Topic.Author.Name;
        this.AuthorId = report.Post.Topic.Author.Id;
        this.Posted = report.Post.Posted;
      }
      else
      {
        this.Subject = report.Post.Subject;
        this.PostFlag = report.Post.Flag;
        this.PostId = report.Post.Id;
        this.Author = report.Post.Author.Name;
        this.AuthorId = report.Post.Author.Id;
        this.Posted = report.Post.Posted;
      }
      this.Content = report.Post.Body;
      this.LastEdited = report.Post.Edited;
      if (!this.LastEdited.HasValue)
        return;
      this.ModeratorChanged = report.Post.ModeratorChanged;
    }

    public int Id { get; set; }

    public string Reason { get; private set; }

    public bool Feedback { get; private set; }

    public DateTimeOffset Timestamp { get; private set; }

    public string ReporterName { get; private set; }

    public int ReporterId { get; private set; }

    [LocalizedDisplay("mvcForum.Web", "PostReportViewModel.Resolved")]
    public bool Resolved { get; set; }

    public string ResolvedByName { get; private set; }

    public int? ResolvedById { get; private set; }

    public DateTimeOffset? ResolvedTimestamp { get; private set; }

    public int TopicId { get; private set; }

    [LocalizedDisplay("mvcForum.Web", "PostReportViewModel.Title")]
    public string Title { get; set; }

    [LocalizedDisplay("mvcForum.Web", "PostReportViewModel.TopicFlag")]
    public TopicFlag TopicFlag { get; set; }

    [LocalizedDisplay("mvcForum.Web", "PostReportViewModel.TopicType")]
    public TopicType TopicType { get; set; }

    public int PostId { get; private set; }

    [LocalizedDisplay("mvcForum.Web", "PostReportViewModel.Subject")]
    public string Subject { get; set; }

    [LocalizedDisplay("mvcForum.Web", "PostReportViewModel.PostFlag")]
    public PostFlag PostFlag { get; set; }

    [LocalizedDisplay("mvcForum.Web", "PostReportViewModel.Content")]
    [Required]
    public string Content { get; set; }

    [LocalizedDisplay("mvcForum.Web", "PostReportViewModel.ChangeReason")]
    public string ChangeReason { get; set; }

    public bool IsTopic { get; private set; }

    public int AuthorId { get; private set; }

    public string Author { get; private set; }

    public DateTime Posted { get; private set; }

    public DateTime? LastEdited { get; private set; }

    public bool ModeratorChanged { get; private set; }
  }
}
