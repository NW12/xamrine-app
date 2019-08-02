package md5061062f279996b10dbc565195f9d88cf;


public class AddNewAddressActivity
	extends android.support.v7.app.AppCompatActivity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("xamarin.android.AddNewAddressActivity, xamarin.android", AddNewAddressActivity.class, __md_methods);
	}


	public AddNewAddressActivity ()
	{
		super ();
		if (getClass () == AddNewAddressActivity.class)
			mono.android.TypeManager.Activate ("xamarin.android.AddNewAddressActivity, xamarin.android", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

	private java.util.ArrayList refList;
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
