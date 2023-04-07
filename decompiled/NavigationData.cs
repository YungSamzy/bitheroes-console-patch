public class NavigationData
{
	public string nav_element_origin;

	public string nav_element_name;

	public string sub_nav_element_name;

	public NavigationData(string origin, string navElementName, string subNavElementName = null)
	{
		nav_element_origin = origin;
		nav_element_name = navElementName;
		sub_nav_element_name = subNavElementName;
	}
}
