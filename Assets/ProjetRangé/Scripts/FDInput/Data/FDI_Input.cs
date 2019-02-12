using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FDI
{
	public class FDI_Input{
		public string name;
		public TypeOfInput typeOfInput ;
		public InputActions inputActions;
		public List<KeyCode> keyCodes;

		///<summary/>
		///Create A FDInput new Input either Axis or Key
		///</summary>
		// public FDI_Input (string _name,  TypeOfInput _tOI)
		public FDI_Input (string _name = "NewInput", TypeOfInput _tOI = TypeOfInput.Key, InputActions _iA = InputActions.Down)
		{
			this.name = _name;
			this.typeOfInput = _tOI;
			this.keyCodes = new List<KeyCode>();
			this.inputActions = _iA;
		}

		public bool InputPressed (List<KeyCode> _kcl)
		{
			foreach (var key in _kcl)
			{
				if(Input.GetKey(key)) return true;
			}
			return false;
		}

		
		public bool InputDown (List<KeyCode> _kcl)
		{
			foreach (var key in _kcl)
			{
				if(Input.GetKey(key)) return true;
			}
			return false;
		}

		
		public bool InputUp (List<KeyCode> _kcl)
		{
			foreach (var key in _kcl)
			{
				if(Input.GetKey(key)) return true;
			}
			return false;
		}
	}

}
