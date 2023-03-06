using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logger : MonoBehaviour {

	private string _moduleName;
	private int _moduleId;
	private bool _hasAssignedModule = false;

	public void AssignModule(string moduleName, int moduleId) {
		if (_hasAssignedModule) {
			throw new InvalidOperationException("Logger already has a module assigned to it!");
		}

		_moduleName = moduleName;
		_moduleId = moduleId;
		_hasAssignedModule = true;
	}

	public void Log(string formattedString) {
		if (!_hasAssignedModule) {
			throw new InvalidOperationException("Logger does not have a module assigned to it!");
		}

		Debug.LogFormat("[{0} #{1}] {2}", _moduleName, _moduleId, formattedString);
    }
}
