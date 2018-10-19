using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test {
	interface IType { }
	class TypeFactory {
		IType Create (int i) {
			if (i == 1) {
				return new Type1 (i, 'c');
			} else {
				return new Type2 (true);
			}
		}
	}
	class Type1 : IType {
		public Type1 (int i, char c) {
			_i = i;
			_c = c;
		}
		int _i;
		char _c;
	}
	class Type2 : IType {
		public Type2 (bool b) {
			_b = b;
		}
		bool _b;
	}
}