/* MVCThemes
 * Copyright (C) 2011-2012 Steen F. Tøttrup
 * http://creativeminds.dk/
 */

using System;
using System.Linq;
using System.Web.Mvc;

using MVCThemes.Interfaces;

namespace MVCThemes {

	public class ThemeViewEngine : RazorViewEngine {
		private IThemeProvider themeProvider = null;
		private IThemeURLProvider urlProvider = null;
		private String lastTheme = String.Empty;

		private String[] amlf = null;
		private String[] apvlf = null;
		private String[] avlf = null;
		private String[] mlf = null;
		private String[] pvlf = null;
		private String[] vlf = null;

		public ThemeViewEngine()
			: base() {
			this.StoreDefaultFormats();
		}
		
		public ThemeViewEngine(IThemeProvider themeProvider, IThemeURLProvider urlProvider)
			: base() {

			this.themeProvider = themeProvider;
			this.urlProvider = urlProvider;

			this.StoreDefaultFormats();
		}

		private IThemeProvider ThemeProvider {
			get {
				if (this.themeProvider == null) {
					this.themeProvider = DependencyResolver.Current.GetService<IThemeProvider>();
				}
				return this.themeProvider;
			}
		}

		private IThemeURLProvider ThemeUrlProvider {
			get {
				if (this.urlProvider == null) {
					this.urlProvider = DependencyResolver.Current.GetService<IThemeURLProvider>();
				}
				return this.urlProvider;
			}
		}

		/// <summary>
		/// Caches the default formats from the razor engine, we'll need these, as we change them base on theme.
		/// </summary>
		private void StoreDefaultFormats() {
			this.pvlf = base.PartialViewLocationFormats;
			this.mlf = base.MasterLocationFormats;
			this.vlf = base.ViewLocationFormats;
			this.amlf = base.AreaMasterLocationFormats;
			this.apvlf = base.AreaPartialViewLocationFormats;
			this.avlf = base.AreaViewLocationFormats;
		}

		private void UpdateLocations(ControllerContext controllerContext) {
			// Get the selected theme.
			String theme = this.ThemeProvider.GetTheme(controllerContext);
			if (!String.IsNullOrWhiteSpace(theme)) {
				if (lastTheme != theme) {
					// Get the base path of the theme.
					String themeBasePath = this.ThemeUrlProvider.GetThemeBaseURL(theme);
					if (!String.IsNullOrWhiteSpace(themeBasePath)) {
						// The needed strings for searching after the (partial) view in the right location.
						String[] themeLocations = new String[] { 
																String.Format("{0}{1}", themeBasePath, "Views/{1}/{0}.cshtml"),
																String.Format("{0}{1}", themeBasePath, "Views/{1}/{0}.vbhtml"),
																String.Format("{0}{1}", themeBasePath, "Views/Shared/{0}.cshtml"),
																String.Format("{0}{1}", themeBasePath, "Views/Shared/{0}.vbhtml"),
																String.Format("{0}{1}", themeBasePath, "Views/Shared/{1}/{0}.cshtml"),
																String.Format("{0}{1}", themeBasePath, "Views/Shared/{1}/{0}.vbhtml")
															};

						String[] themeAreaLocations = new String[] { 
																String.Format("{0}{1}", themeBasePath, "Areas/{2}/Views/{1}/{0}.cshtml"),
																String.Format("{0}{1}", themeBasePath, "Areas/{2}/Views/{1}/{0}.vbhtml"),
																String.Format("{0}{1}", themeBasePath, "Areas/{2}/Views/Shared/{0}.cshtml"),
																String.Format("{0}{1}", themeBasePath, "Areas/{2}/Views/Shared/{0}.vbhtml"),
																String.Format("{0}{1}", themeBasePath, "Areas/{2}/Views/Shared/{1}/{0}.cshtml"),
																String.Format("{0}{1}", themeBasePath, "Areas/{2}/Views/Shared/{1}/{0}.vbhtml")
															};

						// Join the new theme paths with the original formats from the view engine.
						// TODO: All for partial???
						this.AreaMasterLocationFormats = themeAreaLocations.Union(amlf).ToArray();
						this.AreaPartialViewLocationFormats = themeAreaLocations.Union(apvlf).ToArray();
						this.AreaViewLocationFormats = themeAreaLocations.Union(avlf).ToArray();
						this.MasterLocationFormats = themeLocations.Union(mlf).ToArray();
						this.PartialViewLocationFormats = themeLocations.Union(pvlf).ToArray();
						this.ViewLocationFormats = themeLocations.Union(vlf).ToArray();

						lastTheme = theme;
					}
				}
			}
			else {
				// No theme, so let's make sure the formats are the original ones from the view engine.
				// TODO: All for partial???
				this.AreaMasterLocationFormats = amlf;
				this.AreaPartialViewLocationFormats = apvlf;
				this.AreaViewLocationFormats = avlf;
				this.MasterLocationFormats = mlf;
				this.PartialViewLocationFormats = pvlf;
				this.ViewLocationFormats = vlf;

				lastTheme = String.Empty;
			}
		}

		/// <summary>
		/// Finds the specified partial view by using the specified controller context.
		/// </summary>
		/// <exception cref="System.ArgumentNullException"/>
		/// <exception cref="System.ArgumentException"/>
		/// <param name="controllerContext">The controller context.</param>
		/// <param name="partialViewName">The name of the partial view.</param>
		/// <param name="useCache">true to use the cached partial view.</param>
		/// <returns></returns>
		public override ViewEngineResult FindPartialView(ControllerContext controllerContext, String partialViewName, Boolean useCache) {
			// Do the theme trick!
			this.UpdateLocations(controllerContext);
			// Let the razor engine do it's magic, we're done!
			return base.FindPartialView(controllerContext, partialViewName, useCache);
		}

		/// <summary>
		/// Finds the specified view by using the specified controller context and master view name.
		/// </summary>
		/// <exception cref="System.ArgumentNullException"/>
		/// <exception cref="System.ArgumentException"/>
		/// <param name="controllerContext">The controller context.</param>
		/// <param name="viewName">The name of the view.</param>
		/// <param name="masterName">The name of the master view.</param>
		/// <param name="useCache">true to use the cached view.</param>
		/// <returns></returns>
		public override ViewEngineResult FindView(ControllerContext controllerContext, String viewName, String masterName, Boolean useCache) {
			// Do the theme trick!
			this.UpdateLocations(controllerContext);
			// Let the razor engine do it's magic, we're done!
			return base.FindView(controllerContext, viewName, masterName, useCache);
		}

		protected override bool FileExists(ControllerContext controllerContext, string virtualPath) {
			Boolean result = base.FileExists(controllerContext, virtualPath);
			return result;
		}
	}
}