using System;

namespace SimpleLocalisation.Processing {

	public interface IPatternProcessorFactory<TInterface, TArg> {
		TInterface GetFor(TArg rep, PatternDialect dialect/*, TextManager manager*/);
	}
}