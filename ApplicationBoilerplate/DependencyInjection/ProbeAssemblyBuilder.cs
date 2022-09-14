/* MVC Bootstrap
 * Copyright (C) 2012 Steen F. Tøttrup
 * http://mvcbootstrap.codeplex.com/
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ApplicationBoilerplate.DependencyInjection {

	public class ProbeAssemblyBuilder : IDependencyBuilder {
		private readonly Assembly assembly;

		public ProbeAssemblyBuilder(Assembly ass) {
			this.assembly = ass;
		}

		private IEnumerable<IDependencyBuilder> pvtBuilders = null;
		private IEnumerable<IDependencyBuilder> Builders {
			get {
				if (this.pvtBuilders == null) {
					List<IDependencyBuilder> list = new List<IDependencyBuilder>();
					foreach (Type type in assembly.GetTypes()) {
						if (type.GetInterfaces().Contains(typeof(IDependencyBuilder))) {
							list.Add((IDependencyBuilder)Activator.CreateInstance(type));
						}
					}
					this.pvtBuilders = list;
				}
				return pvtBuilders;
			}
		}

		public void Configure(IDependencyContainer container) {
			foreach (IDependencyBuilder builder in this.Builders) {
				try {
					builder.Configure(container);
				}
				catch { }
			}
		}

		public void ValidateRequirements(IList<ApplicationRequirement> feedback) {
			foreach (IDependencyBuilder builder in this.Builders) {
				try {
					builder.ValidateRequirements(feedback);
				}
				catch { }
			}
		}
	}
}