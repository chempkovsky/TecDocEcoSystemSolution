// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.PagedListEx`1
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace TecDocEcoSystemDbClassLibrary
{
  public class PagedListEx<T> : BasePagedList<T>
  {
    private PagedListEx()
    {
    }

    public static async Task<IPagedList<T>> Create(
      IQueryable<T> superset,
      int pageNumber,
      int pageSize)
    {
      PagedListEx<T> list = new PagedListEx<T>();
      await list.Init(superset, pageNumber, pageSize);
      return (IPagedList<T>) list;
    }

    private async Task Init(IQueryable<T> superset, int pageNumber, int pageSize)
    {
      if (pageNumber < 1)
        throw new ArgumentOutOfRangeException(nameof (pageNumber), (object) pageNumber, "PageNumber cannot be below 1.");
      if (pageSize < 1)
        throw new ArgumentOutOfRangeException(nameof (pageSize), (object) pageSize, "PageSize cannot be less than 1.");
      PagedListEx<T> pagedListEx1 = this;
      PagedListEx<T> pagedListEx2;
      int num1;
      if (superset != null)
      {
        pagedListEx2 = pagedListEx1;
        num1 = await superset.CountAsync<T>();
      }
      else
      {
        num1 = 0;
        pagedListEx2 = pagedListEx1;
      }
      pagedListEx2.TotalItemCount = num1;
      this.PageSize = pageSize;
      this.PageNumber = pageNumber;
      this.PageCount = this.TotalItemCount > 0 ? (int) Math.Ceiling((double) this.TotalItemCount / (double) this.PageSize) : 0;
      this.HasPreviousPage = this.PageNumber > 1;
      this.HasNextPage = this.PageNumber < this.PageCount;
      this.IsFirstPage = this.PageNumber == 1;
      this.IsLastPage = this.PageNumber >= this.PageCount;
      this.FirstItemOnPage = (this.PageNumber - 1) * this.PageSize + 1;
      int num = this.FirstItemOnPage + this.PageSize - 1;
      this.LastItemOnPage = num > this.TotalItemCount ? this.TotalItemCount : num;
      if (superset == null || this.TotalItemCount <= 0)
        return;
      List<T> subset = this.Subset;
      List<T> listAsync;
      if (pageNumber != 1)
        listAsync = await superset.Skip<T>((pageNumber - 1) * pageSize).Take<T>(pageSize).ToListAsync<T>();
      else
        listAsync = await superset.Skip<T>(0).Take<T>(pageSize).ToListAsync<T>();
      subset.AddRange((IEnumerable<T>) listAsync);
    }
  }
}
