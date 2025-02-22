namespace GetFieldPropertyValue;

/// <summary>
/// 
/// </summary>
internal record RecordData
{
	/// <summary>
	/// Class field and property names
	/// </summary>
	public readonly string[] ClassFields = [ "Field_01", "Field_02", "Field_03", "Field_04", "Field_05" ];
	public readonly string[] ClassProperties = [ "Property_01", "Property_02" ];

	/// <summary>
	/// Class fields
	/// </summary>
	public string? Field_01;
	public string? Field_02;
	public string? Field_03;
	public string? Field_04;
	public string? Field_05;

	/// <summary>
	/// Class properties
	/// </summary>
	public string? Property_01 { get; set; }
	public string? Property_02 { get; set; }

	/// <summary>
	/// Method to get fields value
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="fieldName"></param>
	/// <returns></returns>
	public T GetFieldValue<T>( string fieldName )
	{
		return ( T ) typeof( RecordData ).GetField( fieldName )!.GetValue( this )!;
	}

	/// <summary>
	/// Method to get properties value
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="propertyName"></param>
	/// <returns></returns>
	public T GetPropertyValue<T>( string propertyName )
	{
		return ( T ) typeof( RecordData ).GetProperty( propertyName )!.GetValue( this )!;
	}
}

/// <summary>
/// 
/// </summary>
public static class Program
{
	/// <summary>
	/// 
	/// </summary>
	public static void Main()
	{
		var record = new RecordData
		{
			Field_01 = "f-one",
			Field_02 = "f-two",
			Field_03 = "f-three",
			Field_04 = "f-four",
			Field_05 = "f-five",
			Property_01 = "p-one",
			Property_02 = "p-two"
		};

		Console.Clear();
		Console.WriteLine( "Class fields:" );

		for ( int i = 0; i < record.ClassFields.Length; i++ )
		{
			Console.WriteLine( $"Value for field_{( i + 1 )} => {
				record.GetFieldValue<string>( record.ClassFields[ i ] )}" );
		}

		Console.WriteLine();
		Console.WriteLine( "Class properties:" );

		for ( int i = 0; i < record.ClassProperties.Length; i++ )
		{
			Console.WriteLine( $"Value for property_{( i + 1 )} => {
				record.GetPropertyValue<string>( record.ClassProperties[ i ] )}" );
		}
	}
}