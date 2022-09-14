// Decompiled with JetBrains decompiler
// Type: MVCBootstrap.Web.Mvc.JsonPResult
// Assembly: MVCBootstrap, Version=0.7.3.31109, Culture=neutral, PublicKeyToken=null
// MVID: DDB5EB46-D133-4D70-972F-AD437AC12CD4
// Assembly location: C:\Development\WebCarShop\MVCBootstrap.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Web.Mvc;

namespace MVCBootstrap.Web.Mvc
{
  public class JsonPResult : ActionResult
  {
    private object data { get; set; }

    public JsonPResult(object data)
    {
      this.data = data;
    }

    public override void ExecuteResult(ControllerContext context)
    {
      context.HttpContext.Response.Write(string.Format("{0}({1});", (object) context.HttpContext.Request["callback"], (object) JsonConvert.SerializeObject(this.data, (JsonConverter) new IsoDateTimeConverter(), (JsonConverter) new StringEnumConverter())));
    }
  }
}
