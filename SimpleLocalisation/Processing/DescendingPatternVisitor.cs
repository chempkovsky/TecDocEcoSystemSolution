﻿using System;
using SimpleLocalisation.Parsing;

namespace SimpleLocalisation.Processing {

	public class DescendingPatternVisitor<T> : IPatternVisitor<T> {

		public virtual void Visit(Expression expression, T state) {
			foreach (var part in expression.Parts) {
				part.Accept(this, state);
			}
		}

		public virtual void Visit(Text text, T state) {

		}

		public virtual void Visit(ParameterSpec spec, T state) {

		}

		public virtual void Visit(Switch sw, T state) {
			if (sw.NullExpression != null) {
				sw.NullExpression.Accept(this, state);
			}

			foreach (var sc in sw.Cases) {
				sc.Accept(this, state);
			}
		}

		public virtual void Visit(SwitchCase sc, T state) {
			if (sc.Condition != null) {
				sc.Condition.Accept(this, state);
			}
			sc.Expression.Accept(this, state);
		}

		public virtual void Visit(FormatGroup group, T state) {
			if (group.Expression != null) {
				group.Expression.Accept(this, state);
			}
		}

		public virtual void Visit(CustomExpressionPart part, T state) {
		}

		public virtual T CreateInitialState() {
			return default(T);
		}
	}
}