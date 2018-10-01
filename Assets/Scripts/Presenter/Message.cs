using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UniRx;
using UnityEngine;

public interface Message { }
public class ReplenishedMsg : Message {
	public readonly (Transform source, Transform target) msg;
}
public class PlayedMsg : Message {
	public readonly (Transform source, Transform target) msg;
}