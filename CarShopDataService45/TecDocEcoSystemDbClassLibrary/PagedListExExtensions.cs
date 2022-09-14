// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.PagedListExExtensions
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using PagedList;
using System.Linq;
using System.Threading.Tasks;

namespace TecDocEcoSystemDbClassLibrary
{
  public static class PagedListExExtensions
  {
    public static async Task<IPagedList<T>> ToPagedListAsync<T>(
      this IQueryable<T> superset,
      int pageNumber,
      int pageSize)
    {
      return await PagedListEx<T>.Create(superset, pageNumber, pageSize);
    }
  }
}
