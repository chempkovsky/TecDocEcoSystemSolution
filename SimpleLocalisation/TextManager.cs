using System;
using System.Collections.Generic;
using System.Linq;

using SimpleLocalisation.Processing;
using SimpleLocalisation.Support;

namespace SimpleLocalisation {

	public class TextManager {
		private readonly ICultureContext context;
		private readonly ITextSource source;
		//private readonly <Language> fallbacks = new List<Language>();

		public IEnumerable<Language> FallbackLanguages { get; set; }

		public Func<String, String> StringEncoder { get; set; }

		public TextManager(ICultureContext context, ITextSource textSource, Language[] fallbackLanguages) {
			this.context = context;
			this.source = textSource;

			if (fallbackLanguages == null || (fallbackLanguages != null && !fallbackLanguages.Any())) {
				throw new ArgumentNullException("fallbackLanguages");
			}

			this.FallbackLanguages = fallbackLanguages.ToList();
			// TODO: !??!?!
			this.StringEncoder = (s) =>
				s ?? ""
				.Replace("\r\n", "\n") // Windows to UNIX
				.Replace("\r", "<br />") //Mac
				.Replace("\n", "<br />"); //Other

			this.Dialects = new Dictionary<String, PatternDialect>();
			this.Dialects.Add("Default", new DefaultDialect());
			this.Dialects.Add("Text", new TextDialect());
		}

		public Language GetCurrentLanguage() {
			return this.context.Language;
		}

		public TimeZoneInfo GetCurrentTimeZone() {
			return this.context.TimeZone;
		}

		public String Get<TNamespace>(String key, Object values = null) {
			return this.Get(key, ns: typeof(TNamespace).Namespace, values: values);
		}

		private LocalisedTextEntry InternalGet(String key, String ns, ParameterSet param, Language language) {
			// Try getting a text for the current language (according to the context object).
			LocalisedTextEntry entry = this.source.Get(key, ns, this.context.Language);
			if (entry == null) {
				// No text found, let's try getting one for any of the fall-back languages!
				List<Language> fallbacks = new List<Language>();
				if (this.FallbackLanguages != null) {
					fallbacks.AddRange(this.FallbackLanguages.Reverse());
				}

				if (fallbacks.Any(x => (entry = this.source.Get(key, ns, x)) != null)) {
					return entry;
				}
			}

			return entry;
		}

		public String Get(String key, Object values = null, String ns = null) {
			ParameterSet dict = ObjectHelper.ParamsToParameterSet(values, addWithIndex: true);

			LocalisedTextEntry entry = this.InternalGet(key, ns ?? "", dict, this.context.Language);
			if (entry == null) {
				return String.Empty;
			}

			this.EnsureEvaluator(entry);

			return entry.Evaluator.Evaluate(new EvaluationContext {
				Parameters = dict,
				Language = this.context.Language,
				TimeZoneInfo = this.context.TimeZone,
				Namespace = ns ?? "",
				StringEncoder = StringEncoder != null && entry.PatternDialect.Encode ? StringEncoder : ((x) => x) //Use identity transform if no transformer is specified
			});
		}

		public IDictionary<String, PatternDialect> Dialects { get; private set; }

		protected void EnsureEvaluator(LocalisedTextEntry entry) {
			if (entry.Evaluator == null) {
				PatternDialect dialect;
				if (this.Dialects.TryGetValue(entry.Text.PatternDialect, out dialect)) {
					try {
						entry.PatternDialect = dialect;
						entry.Evaluator = dialect.GetEvaluator(entry.Text.Pattern);
					}
					catch {

						//var parameters = new {
						//    Message = pe.Message,
						//    Namespace = entry.Text.Namespace,
						//    Key = entry.Text.Key,
						//    Language = entry.Text.Language
						//};

						//var ex = new PatternException("TextManager.PatternException",
						//    "{0} while parsing {2} in namespace \"{1}\" for {2}", parameters, innerException: pe);

						// TODO:
						throw new ApplicationException();
					}
				}
				else {
					throw new ApplicationException(entry.Text.PatternDialect);
				}
			}
		}

		public IEnumerable<LocalisedText> Texts {
			get {
				return this.source.Get();
			}
		}
	}
}