
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Cmdarg

	/// <summary>
	/// Cmdarg object for NHibernate mapped table 'cmd_arg'.
	/// </summary>
	public partial  class Cmdarg
		{
		#region Member Variables
		
		protected long? _id;
		protected string _sentence;
		protected int? _sentenceindex;
		protected Cmdqueueitem _cmdqueueitem = new Cmdqueueitem();
		
		

		#endregion

		#region Constructors

		public Cmdarg() { }

		public Cmdarg( string sentence, int? sentenceindex, Cmdqueueitem cmdqueueitem )
		{
			this._sentence = sentence;
			this._sentenceindex = sentenceindex;
			this._cmdqueueitem = cmdqueueitem;
		}

		#endregion

		#region Public Properties

		public long? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public string Sentence
		{
			get { return _sentence; }
			set
			{
				if ( value != null && value.Length > 255)
					throw new ArgumentOutOfRangeException("Invalid value for Sentence", value, value.ToString());
				_sentence = value;
			}
		}

		public int? Sentenceindex
		{
			get { return _sentenceindex; }
			set { _sentenceindex = value; }
		}

		public Cmdqueueitem Cmdqueueitem
		{
			get { return _cmdqueueitem; }
			set { _cmdqueueitem = value; }
		}


		#endregion
		
	}

	#endregion
}



