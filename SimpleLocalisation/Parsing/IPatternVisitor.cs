using System;

namespace SimpleLocalisation.Parsing {

	public interface IPatternVisitor<T> {

		void Visit(Expression expression, T state);

		void Visit(Text text, T state);

		void Visit(ParameterSpec spec, T state);

		void Visit(Switch sw, T state);

		void Visit(SwitchCase sc, T state);

		void Visit(FormatGroup group, T state);

		void Visit(CustomExpressionPart part, T state);

		T CreateInitialState();
	}
}