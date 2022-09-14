// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.PostReport
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

using System;
using System.ComponentModel.DataAnnotations;

namespace mvcForum.Core
{
  public class PostReport
  {
    private DateTime timestamp;
    private DateTime? resolvedTimestamp;

    public PostReport()
    {
    }

    public PostReport(Post post, string reason, ForumUser reportedBy)
      : this(post, reason, reportedBy, false)
    {
    }

    public PostReport(Post post, string reason, ForumUser reportedBy, bool feedback)
    {
      this.Post = post;
      this.Reason = reason;
      this.Timestamp = DateTime.UtcNow;
      this.Resolved = false;
      if (reportedBy != null)
        this.ReportedBy = reportedBy;
      this.Feedback = feedback;
    }

    public int Id { get; set; }

    [Required]
    public int PostId { get; set; }

    public virtual Post Post { get; set; }

    [Required]
    public DateTime Timestamp
    {
      get
      {
        return this.timestamp;
      }
      set
      {
        this.timestamp = value.Handle();
      }
    }

    [StringLength(500)]
    [Required]
    public string Reason { get; set; }

    [Required]
    public int ReportedById { get; set; }

    public virtual ForumUser ReportedBy { get; set; }

    [Required]
    public bool Feedback { get; set; }

    [Required]
    public bool Resolved { get; set; }

    public int? ResolvedById { get; set; }

    public virtual ForumUser ResolvedBy { get; set; }

    public DateTime? ResolvedTimestamp
    {
      get
      {
        return this.resolvedTimestamp;
      }
      set
      {
        this.resolvedTimestamp = value.Handle();
      }
    }
  }
}
