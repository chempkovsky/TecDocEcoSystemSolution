using System;
using System.Collections.Generic;
using System.Text;

using ApplicationBoilerplate.DependencyInjection;

namespace MVCBootstrap {

	public class ApplicationInitializer {
		private static Object initLock = new Object();
		private static Boolean initialized = false;

		public ApplicationInitializer(IDependencyContainer container, IDependencyBuilder[] builders) {

			if (!initialized) {
				lock (initLock) {
					if (!initialized) {
						container.Configure();

						this.ValidateRequirements(builders);

						foreach (IDependencyBuilder builder in builders) {
							builder.Configure(container);
						}

						initialized = true;
					}
				}
			}
		}

		private void ValidateRequirements(IDependencyBuilder[] builders) {
			List<ApplicationRequirement> feedback = new List<ApplicationRequirement>();

			foreach (IDependencyBuilder builder in builders) {
				builder.ValidateRequirements(feedback);
			}

			Boolean fatalFound = false;
			StringBuilder feedbacks = new StringBuilder();
			foreach (ApplicationRequirement requirement in feedback) {
				if (requirement.Level == RequirementLevel.Fatal) {
					fatalFound = true;
				}
				feedbacks.AppendLine(requirement.Feedback);
			}
			if (fatalFound) {
				String exceptionText = "One or more fatal errors found during initialization, please look at the trace for details (visit: /trace.axd).";
				//if (!app.Context.Trace.IsEnabled) {
				exceptionText = @"Tracing is disabled on this web application, please enable it by adding this to the web.config file:
<configuration>
	<system.web>
		<trace enabled=""true""/>
	</system.web>
</configuration>

" + exceptionText + Environment.NewLine + feedbacks.ToString();
				//}
				throw new ApplicationException(exceptionText);
			}
		}
	}
}