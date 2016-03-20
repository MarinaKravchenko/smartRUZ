package md52e84f7766da592023532c7014e82912b;


public class CalendarActivity
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("SmartScheduler.CalendarActivity, SmartScheduler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", CalendarActivity.class, __md_methods);
	}


	public CalendarActivity () throws java.lang.Throwable
	{
		super ();
		if (getClass () == CalendarActivity.class)
			mono.android.TypeManager.Activate ("SmartScheduler.CalendarActivity, SmartScheduler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

	java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
