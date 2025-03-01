using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace GetFieldPropertyValue;

/// <summary>
/// 
/// </summary>
public record RecordData
{
	/// <summary>
	/// Class fields
	/// </summary>
	public string? StringField;
	public int IntField;
	public decimal DecimalField;
	public float FloatField;
	public bool BoolField;

	/// <summary>
	/// Class field names
	/// </summary>
	public readonly Dictionary<string, Type> FieldNameTypes = new()
	{
		[ "StringField" ] = typeof( string ),
		[ "IntField" ] = typeof( int ),
		[ "DecimalField" ] = typeof( decimal ),
		[ "FloatField" ] = typeof( float ),
		[ "BoolField" ] = typeof( bool )
	};

	/// <summary>
	/// Class properties
	/// </summary>
	public string? StringProperty { get; set; }
	public int IntProperty { get; set; }
	public decimal DecimalProperty { get; set; }
	public float FloatProperty { get; set; }
	public bool BoolProperty { get; set; }

	/// <summary>
	/// Class property names
	/// </summary>
	public readonly Dictionary<string, Type> PropertyNameTypes = new()
	{
		[ "StringProperty" ] = typeof( string ),
		[ "IntProperty" ] = typeof( int ),
		[ "DecimalProperty" ] = typeof( decimal ),
		[ "FloatProperty" ] = typeof( float ),
		[ "BoolProperty" ] = typeof( bool )
	};

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
/// |----------------------------------------------------------------------------------------:|
/// | Method                                    | Mean     | Error     | StdDev    | Median   |
/// |------------------------------------------ |---------:|----------:|----------:|---------:|
/// | GetFieldsByOnlyReflection                 | 1.442 us | 0.0182 us | 0.0170 us | 1.444 us |
/// | GetPropsByOnlyReflection                  | 1.175 us | 0.0156 us | 0.0138 us | 1.173 us |
/// |------------------------------------------ |---------:|----------:|----------:|---------:|
/// | GetFieldsByGenericAndReflection           | 4.366 us | 0.0561 us | 0.0525 us | 4.371 us |
/// | GetPropsByGenericAndReflection            | 4.701 us | 0.2165 us | 0.6314 us | 4.537 us |
/// | GetFieldsByGenericReflectionAndDictionary | 4.587 us | 0.2291 us | 0.6684 us | 4.319 us |
/// | GetPropsByGenericReflectionAndDictionary  | 4.206 us | 0.1623 us | 0.4579 us | 4.187 us |
/// |------------------------------------------ |---------:|----------:|----------:|---------:|
/// | * Legends *                                                                             |
/// | Mean   : Arithmetic mean of all measurements                                            |
/// | Error  : Half of 99.9% confidence interval                                              |
/// | StdDev : Standard deviation of all measurements                                         |
/// | Median : Value separating the higher half of all measurements(50th percentile)          |
/// | 1 us   : 1 Microsecond(0.000001 sec)                                                    |
/// |----------------------------------------------------------------------------------------:|
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
			StringField = "Hello",
			IntField = 247,
			DecimalField = 120.34m,
			FloatField = 3.014f,
			BoolField = true,
			StringProperty = "World",
			IntProperty = 742,
			DecimalProperty = 430.21m,
			FloatProperty = 4.103f,
			BoolProperty = false
		};
	}

	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	[Benchmark]
	public string GetFieldsByOnlyReflection()
	{
		var fields = typeof( RecordData ).GetFields();
		string result = string.Empty;

		foreach ( var field in fields )
		{
			// Descartamos los 2 campos adicionales, definidos por "Reflection"
			if ( !field.IsInitOnly )
			{
				var fldValue = typeof( RecordData )
					.GetField( field.Name )!
					.GetValue( _record )!;
				
				result = string.Concat( result, field.Name, " = ", fldValue, ", " );
			}
		}

		return result;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	[Benchmark]
	public string GetPropsByOnlyReflection()
	{
		var properties = typeof( RecordData ).GetProperties();
		string result = string.Empty;

		foreach ( var property in properties )
		{
			var propValue = typeof( RecordData )
				.GetProperty( property.Name )!
				.GetValue( _record )!;
			
			result = string.Concat( result, property.Name, " = ", propValue, ", " );
		}

		return result;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	[Benchmark]
	public string GetFieldsByGenericAndReflection()
	{
		string result = string.Empty;

		var fields = typeof( RecordData ).GetFields();

		foreach ( var field in fields )
		{
			// Descartamos los 2 campos adicionales, definidos por "Reflection"
			if ( !field.IsInitOnly )
			{
				var fieldValue = typeof( RecordData )
					.GetMethod( "GetFieldValue" )!
					.MakeGenericMethod( field.FieldType )
					.Invoke( _record, [ field.Name ] );

				result = string.Concat( result, field.Name, " = ", fieldValue, ", " );
			}
		}

		return result;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	[Benchmark]
	public string GetPropsByGenericAndReflection()
	{
		string result = string.Empty;

		var properties = typeof( RecordData ).GetProperties();

		foreach ( var property in properties )
		{
			var propertyValue = typeof( RecordData )
				.GetMethod( "GetPropertyValue" )!
				.MakeGenericMethod( property.PropertyType )
				.Invoke( _record, [ property.Name ] );

			result = string.Concat( result, property.Name, " = ", propertyValue, ", " );
		}

		return result;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	[Benchmark]
	public string GetFieldsByGenericReflectionAndDictionary()
	{
		string result = string.Empty;

		foreach ( var field in _record.FieldNameTypes )
		{
			var fieldValue = typeof( RecordData )
				.GetMethod( "GetFieldValue" )!
				.MakeGenericMethod( field.Value )
				.Invoke( _record, [ field.Key ] );

			result = string.Concat( result, field.Key, " = ", fieldValue, ", " );
		}

		return result;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	[Benchmark]
	public string GetPropsByGenericReflectionAndDictionary()
	{
		string result = string.Empty;

		foreach ( var property in _record.PropertyNameTypes )
		{
			var propertyValue = typeof( RecordData )
				.GetMethod( "GetPropertyValue" )!
				.MakeGenericMethod( property.Value )
				.Invoke( _record, [ property.Key ] );

			result = string.Concat( result, property.Key, " = ", propertyValue, ", " );
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
	public static void Main( string[] args )
	{
		// Run Class Benchmark
		if ( ( args.Length > 0 ) && args[ 0 ].Equals( "/benchmark",
			StringComparison.InvariantCultureIgnoreCase ) )
		{
			BenchmarkRunner.Run<BenchmarkClass>();
		}
		else
		{
			// Run process with console output
			var record = new RecordData
			{
				StringField = "Hello",
				IntField = 247,
				DecimalField = 120.34m,
				FloatField = 3.014f,
				BoolField = true,
				StringProperty = "World",
				IntProperty = 742,
				DecimalProperty = 430.21m,
				FloatProperty = 4.103f,
				BoolProperty = false
			};

			Console.Clear();
			Console.WriteLine( "Class fields:" );

			var fields = typeof( RecordData ).GetFields();

			foreach ( var field in fields )
			{
				// Descartamos los 2 campos adicionales, definidos por "Reflection"
				if ( !field.IsInitOnly )
				{
					var fieldValue = typeof( RecordData )
					.GetField( field.Name )!
					.GetValue( record )!;

					Console.WriteLine( $"Value of '{field.Name}' =>  {fieldValue}" );
				}
			}

			Console.WriteLine();
			Console.WriteLine( "Class properties:" );

			var properties = typeof( RecordData ).GetProperties();

			foreach ( var property in properties )
			{
				var propertyValue = typeof( RecordData )
					.GetProperty( property.Name )!
					.GetValue( record )!;
				
				Console.WriteLine( $"Value of '{property.Name}' => {propertyValue}" );
			}
		}
	}
}