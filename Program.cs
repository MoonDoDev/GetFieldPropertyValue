using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

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

	public readonly List<string> FieldList = [ "Field_01", "Field_02", "Field_03", "Field_04", "Field_05" ];
	public readonly List<string> PropertyList = [ "Property_01", "Property_02" ];

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
	internal T GetFieldValue<T>( string fieldName )
	{
		return ( T ) typeof( RecordData ).GetField( fieldName )!.GetValue( this )!;
	}

	/// <summary>
	/// Method to get properties value
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="propertyName"></param>
	/// <returns></returns>
	internal T GetPropertyValue<T>( string propertyName )
	{
		return ( T ) typeof( RecordData ).GetProperty( propertyName )!.GetValue( this )!;
	}
}

/// <summary>
/// |---------------------------------------------------------|
/// | Method                 | Mean     | Error    | StdDev   |
/// |----------------------- |---------:|---------:|---------:|
/// | GetFieldsByFor         | 778.3 ns | 14.65 ns | 14.39 ns |
/// | GetPropertiesByFor     | 259.2 ns |  5.30 ns |  7.26 ns |
/// | GetFieldsByForeach     | 798.3 ns | 28.84 ns | 85.03 ns |
/// | GetPropertiesByForeach | 278.5 ns | 12.58 ns | 36.69 ns |
/// |---------------------------------------------------------|
/// | * Legends *                                             |
/// | Mean   : Arithmetic mean of all measurements            |
/// | Error  : Half of 99.9% confidence interval              |
/// | StdDev : Standard deviation of all measurements         |
/// | 1 ns   : 1 Nanosecond(0.000000001 sec)                  |
/// |---------------------------------------------------------|
/// </summary>
public class BenchmarkClass
{
	private readonly RecordData _record;

	/// <summary>
	/// 
	/// </summary>
	public BenchmarkClass()
	{
		_record = new()
		{
			Field_01 = "f-one",
			Field_02 = "f-two",
			Field_03 = "f-three",
			Field_04 = "f-four",
			Field_05 = "f-five",
			Property_01 = "p-one",
			Property_02 = "p-two"
		};
	}

	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	[Benchmark]
	public string GetFieldsByFor()
	{
		string result = string.Empty;

		for ( int i = 0; i < _record.ClassFields.Length; i++ )
		{
			result = string.Concat( result, _record.ClassFields[ i ], " = ",
				_record.GetFieldValue<string>( _record.ClassFields[ i ] ), ", " );
		}

		return result;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	[Benchmark]
	public string GetPropertiesByFor()
	{
		string result = string.Empty;

		for ( int i = 0; i < _record.ClassProperties.Length; i++ )
		{
			result = string.Concat( result, _record.ClassProperties[ i ], " = ",
				_record.GetPropertyValue<string>( _record.ClassProperties[ i ] ), ", " );
		}

		return result;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	[Benchmark]
	public string GetFieldsByForeach()
	{
		string result = string.Empty;

		foreach ( var field in _record.FieldList )
		{
			result = string.Concat( result, field, " = ",
				_record.GetFieldValue<string>( field ), ", " );
		}

		return result;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	[Benchmark]
	public string GetPropertiesByForeach()
	{
		string result = string.Empty;

		foreach ( var property in _record.PropertyList )
		{
			result = string.Concat( result, property, " = ",
				_record.GetPropertyValue<string>( property ), ", " );
		}

		return result;
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
		// Run Class Benchmark
		BenchmarkRunner.Run<BenchmarkClass>();

		// Run process with console output
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
		Console.WriteLine( "Class fields ( string array ):" );

		for ( int i = 0; i < record.ClassFields.Length; i++ )
		{
			Console.WriteLine( $"Value for field_{( i + 1 )} => {record.GetFieldValue<string>( record.ClassFields[ i ] )}" );
		}

		Console.WriteLine();
		Console.WriteLine( "Class properties ( string array ):" );

		for ( int i = 0; i < record.ClassProperties.Length; i++ )
		{
			Console.WriteLine( $"Value for property_{( i + 1 )} => {record.GetPropertyValue<string>( record.ClassProperties[ i ] )}" );
		}

		Console.WriteLine();
		Console.WriteLine( "Class fields ( string list ):" );

		foreach ( var field in record.FieldList )
		{
			Console.WriteLine( $"Value for {field} => {record.GetFieldValue<string>( field )}" );
		}

		Console.WriteLine();
		Console.WriteLine( "Class properties ( string list ):" );

		foreach ( var property in record.PropertyList )
		{
			Console.WriteLine( $"Value for {property} => {record.GetPropertyValue<string>( property )}" );
		}
	}
}