using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UniRx;
using UnityEngine;

public interface IMessage { }
public abstract class MessageBody<T> {
	public MessageBody (T body) {
		this.body = body;
	}
	public readonly T body;
}
public class ReplenisheMsg : MessageBody<Card>, IMessage {
	public ReplenisheMsg (Card body) : base (body) { }
}
public class ShuffleMsg : MessageBody<IList<Card>>, IMessage {
	public ShuffleMsg (IList<Card> body) : base (body) { }
}

public class PlayMsg : MessageBody<IList<Card>>, IMessage {
	public PlayMsg (IList<Card> body) : base (body) { }
}

public class FirstPutMsg : MessageBody<Card>, IMessage {
	public FirstPutMsg (Card body) : base (body) { }
}

public class DiscardMsg : MessageBody<IList<IList<Card>>>, IMessage {
	public DiscardMsg (IList<IList<Card>> body) : base (body) { }
}