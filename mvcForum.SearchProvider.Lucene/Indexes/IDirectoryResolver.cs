// Decompiled with JetBrains decompiler
// Type: mvcForum.SearchProvider.Lucene.Indexes.IDirectoryResolver
// Assembly: mvcForum.SearchProvider.Lucene, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 13A5DCA9-908F-4689-9C6D-F584F417B850
// Assembly location: C:\Development\WebCarShop\mvcForum.SearchProvider.Lucene.dll


using Lucene.Net.Store;

namespace mvcForum.SearchProvider.Lucene.Indexes
{
  public interface IDirectoryResolver
  {
        Directory GetDirectory();
  }
}
