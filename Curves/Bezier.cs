﻿// by Freya Holmér (https://github.com/FreyaHolmer/Mathfs)
// a lot of stuff here made possible by this excellent writeup on bezier curves: https://pomax.github.io/bezierinfo/

using System;
using UnityEngine;
using static Freya.Mathfs;

namespace Freya {

	// Bezier math
	// A lot of the following code is unrolled into floats and components for performance reasons.
	// It's much faster than keeping the more readable function calls and vector types unfortunately

	/// <summary>A 2D cubic bezier curve, with 4 control points</summary>
	[Serializable] public partial struct BezierCubic2D {

		/// <summary>The starting point of the curve</summary>
		public Vector2 p0;

		/// <summary>The second control point of the curve, sometimes called the start tangent point</summary>
		public Vector2 p1;

		/// <summary>The third control point of the curve, sometimes called the end tangent point</summary>
		public Vector2 p2;

		/// <summary>The end point of the curve</summary>
		public Vector2 p3;

		/// <summary>Creates a cubic bezier curve, from 4 control points</summary>
		/// <param name="p0">The starting point of the curve</param>
		/// <param name="p1">The second control point of the curve, sometimes called the start tangent point</param>
		/// <param name="p2">The third control point of the curve, sometimes called the end tangent point</param>
		/// <param name="p3">The end point of the curve</param>
		public BezierCubic2D( Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3 ) => ( this.p0, this.p1, this.p2, this.p3 ) = ( p0, p1, p2, p3 );

		/// <summary>Returns a control point position by index. Valid indices: 0, 1, 2 or 3</summary>
		public Vector2 this[ int i ] {
			get {
				switch( i ) {
					case 0:  return p0;
					case 1:  return p1;
					case 2:  return p2;
					case 3:  return p3;
					default: throw new ArgumentOutOfRangeException( nameof(i), $"Index has to be in the 0 to 3 range, and I think {i} is outside that range you know" );
				}
			}
			set {
				switch( i ) {
					case 0:
						p0 = value;
						break;
					case 1:
						p1 = value;
						break;
					case 2:
						p2 = value;
						break;
					case 3:
						p3 = value;
						break;
					default: throw new ArgumentOutOfRangeException( nameof(i), $"Index has to be in the 0 to 3 range, and I think {i} is outside that range you know" );
				}
			}
		}

	}

	/// <summary>A 3D cubic bezier curve, with 4 control points</summary>
	[Serializable] public partial struct BezierCubic3D {

		/// <inheritdoc cref="BezierCubic2D.p0"/>
		public Vector3 p0;

		/// <inheritdoc cref="BezierCubic2D.p1"/>
		public Vector3 p1;

		/// <inheritdoc cref="BezierCubic2D.p2"/>
		public Vector3 p2;

		/// <inheritdoc cref="BezierCubic2D.p3"/>
		public Vector3 p3;

		/// <inheritdoc cref="BezierCubic2D(Vector2,Vector2,Vector2,Vector2)"/>
		/// <param name="p0">The starting point of the curve</param>
		/// <param name="p1">The second control point of the curve, sometimes called the start tangent point</param>
		/// <param name="p2">The third control point of the curve, sometimes called the end tangent point</param>
		/// <param name="p3">The end point of the curve</param>
		public BezierCubic3D( Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3 ) => ( this.p0, this.p1, this.p2, this.p3 ) = ( p0, p1, p2, p3 );

		/// <inheritdoc cref="BezierCubic2D.this"/> 
		public Vector3 this[ int i ] {
			get {
				switch( i ) {
					case 0:  return p0;
					case 1:  return p1;
					case 2:  return p2;
					case 3:  return p3;
					default: throw new IndexOutOfRangeException();
				}
			}
			set {
				switch( i ) {
					case 0:
						p0 = value;
						break;
					case 1:
						p1 = value;
						break;
					case 2:
						p2 = value;
						break;
					case 3:
						p3 = value;
						break;
					default: throw new IndexOutOfRangeException();
				}
			}
		}

	}

	#region Object Comparison

	public partial struct BezierCubic2D {

		public static bool operator ==( BezierCubic2D a, BezierCubic2D b ) => a.p0 == b.p0 && a.p1 == b.p1 && a.p2 == b.p2 && a.p3 == b.p3;
		public static bool operator !=( BezierCubic2D a, BezierCubic2D b ) => !( a == b );
		public bool Equals( BezierCubic2D other ) => p0.Equals( other.p0 ) && p1.Equals( other.p1 ) && p2.Equals( other.p2 ) && p3.Equals( other.p3 );
		public override bool Equals( object obj ) => obj is BezierCubic2D other && Equals( other );

		public override int GetHashCode() {
			unchecked {
				int hashCode = p0.GetHashCode();
				hashCode = ( hashCode * 397 ) ^ p1.GetHashCode();
				hashCode = ( hashCode * 397 ) ^ p2.GetHashCode();
				hashCode = ( hashCode * 397 ) ^ p3.GetHashCode();
				return hashCode;
			}
		}


	}

	public partial struct BezierCubic3D {

		public static bool operator ==( BezierCubic3D a, BezierCubic3D b ) => a.p0 == b.p0 && a.p1 == b.p1 && a.p2 == b.p2 && a.p3 == b.p3;
		public static bool operator !=( BezierCubic3D a, BezierCubic3D b ) => !( a == b );
		public bool Equals( BezierCubic3D other ) => p0.Equals( other.p0 ) && p1.Equals( other.p1 ) && p2.Equals( other.p2 ) && p3.Equals( other.p3 );
		public override bool Equals( object obj ) => obj is BezierCubic3D other && Equals( other );

		public override int GetHashCode() {
			unchecked {
				int hashCode = p0.GetHashCode();
				hashCode = ( hashCode * 397 ) ^ p1.GetHashCode();
				hashCode = ( hashCode * 397 ) ^ p2.GetHashCode();
				hashCode = ( hashCode * 397 ) ^ p3.GetHashCode();
				return hashCode;
			}
		}
	}

	#endregion

	// Base properties - Points, Derivatives & Tangents

	#region Point

	public partial struct BezierCubic2D {
		/// <summary>Returns the point at the given t-value on the curve</summary>
		/// <param name="t">The t-value along the curve to sample</param>
		public Vector2 GetPoint( float t ) {
			float ax = p0.x + ( p1.x - p0.x ) * t; // a = lerp( p0, p1, t );
			float ay = p0.y + ( p1.y - p0.y ) * t;
			float bx = p1.x + ( p2.x - p1.x ) * t; // b = lerp( p1, p2, t );
			float by = p1.y + ( p2.y - p1.y ) * t;
			float cx = p2.x + ( p3.x - p2.x ) * t; // c = lerp( p2, p3, t );
			float cy = p2.y + ( p3.y - p2.y ) * t;
			float dx = ax + ( bx - ax ) * t; // d = lerp( a, b, t );
			float dy = ay + ( by - ay ) * t;
			float ex = bx + ( cx - bx ) * t; // e = lerp( b, c, t );
			float ey = by + ( cy - by ) * t;
			return new Vector2( // ret lerp( d, e, t );
				dx + ( ex - dx ) * t,
				dy + ( ey - dy ) * t
			);
		}
	}

	public partial struct BezierCubic3D {
		/// <summary>Returns the point at the given t-value on the curve</summary>
		/// <param name="t">The t-value along the curve to sample</param>
		public Vector3 GetPoint( float t ) {
			float ax = p0.x + ( p1.x - p0.x ) * t; // a = lerp( p0, p1, t );
			float ay = p0.y + ( p1.y - p0.y ) * t;
			float az = p0.z + ( p1.z - p0.z ) * t;
			float bx = p1.x + ( p2.x - p1.x ) * t; // b = lerp( p1, p2, t );
			float by = p1.y + ( p2.y - p1.y ) * t;
			float bz = p1.z + ( p2.z - p1.z ) * t;
			float cx = p2.x + ( p3.x - p2.x ) * t; // c = lerp( p2, p3, t );
			float cy = p2.y + ( p3.y - p2.y ) * t;
			float cz = p2.z + ( p3.z - p2.z ) * t;
			float dx = ax + ( bx - ax ) * t; // d = lerp( a, b, t );
			float dy = ay + ( by - ay ) * t;
			float dz = az + ( bz - az ) * t;
			float ex = bx + ( cx - bx ) * t; // e = lerp( b, c, t );
			float ey = by + ( cy - by ) * t;
			float ez = bz + ( cz - bz ) * t;
			return new Vector3( // ret Vector3.LerpUnclamped( d, e, t );
				dx + ( ex - dx ) * t,
				dy + ( ey - dy ) * t,
				dz + ( ez - dz ) * t
			);
		}
	}

	#endregion

	#region Point Components

	public partial struct BezierCubic2D {
		/// <summary>Returns the X coordinate at the given t-value on the curve</summary>
		/// <param name="t">The t-value along the curve to sample</param>
		public float GetPointX( float t ) {
			float a = p0.x + ( p1.x - p0.x ) * t; // a = lerp( p0, p1, t );
			float b = p1.x + ( p2.x - p1.x ) * t; // b = lerp( p1, p2, t );
			float c = p2.x + ( p3.x - p2.x ) * t; // c = lerp( p2, p3, t );
			float d = a + ( b - a ) * t; // d = lerp( a, b, t );
			float e = b + ( c - b ) * t; // e = lerp( b, c, t );
			return d + ( e - d ) * t; // ret lerp( d, e, t );
		}

		/// <summary>Returns the Y coordinate at the given t-value on the curve</summary>
		/// <param name="t">The t-value along the curve to sample</param>
		public float GetPointY( float t ) {
			float a = p0.y + ( p1.y - p0.y ) * t; // a = lerp( p0, p1, t );
			float b = p1.y + ( p2.y - p1.y ) * t; // b = lerp( p1, p2, t );
			float c = p2.y + ( p3.y - p2.y ) * t; // c = lerp( p2, p3, t );
			float d = a + ( b - a ) * t; // d = lerp( a, b, t );
			float e = b + ( c - b ) * t; // e = lerp( b, c, t );
			return d + ( e - d ) * t; // ret lerp( d, e, t );
		}

		/// <summary>Returns a component of the coordinate at the given t-value on the curve</summary>
		/// <param name="component">Which component of the coordinate to return. 0 is X, 1 is Y</param>
		/// <param name="t">The t-value along the curve to sample</param>
		public float GetPointComponent( int component, float t ) {
			switch( component ) {
				case 0:  return GetPointX( t );
				case 1:  return GetPointY( t );
				default: throw new ArgumentOutOfRangeException( nameof(component), "component has to be either 0 or 1" );
			}
		}
	}

	public partial struct BezierCubic3D {
		/// <summary>Returns the X coordinate at the given t-value on the curve</summary>
		/// <param name="t">The t-value along the curve to sample</param>
		public float GetPointX( float t ) {
			float a = p0.x + ( p1.x - p0.x ) * t; // a = lerp( p0, p1, t );
			float b = p1.x + ( p2.x - p1.x ) * t; // b = lerp( p1, p2, t );
			float c = p2.x + ( p3.x - p2.x ) * t; // c = lerp( p2, p3, t );
			float d = a + ( b - a ) * t; // d = lerp( a, b, t );
			float e = b + ( c - b ) * t; // e = lerp( b, c, t );
			return d + ( e - d ) * t; // ret lerp( d, e, t );
		}

		/// <summary>Returns the Y coordinate at the given t-value on the curve</summary>
		/// <param name="t">The t-value along the curve to sample</param>
		public float GetPointY( float t ) {
			float a = p0.y + ( p1.y - p0.y ) * t; // a = lerp( p0, p1, t );
			float b = p1.y + ( p2.y - p1.y ) * t; // b = lerp( p1, p2, t );
			float c = p2.y + ( p3.y - p2.y ) * t; // c = lerp( p2, p3, t );
			float d = a + ( b - a ) * t; // d = lerp( a, b, t );
			float e = b + ( c - b ) * t; // e = lerp( b, c, t );
			return d + ( e - d ) * t; // ret lerp( d, e, t );
		}

		/// <summary>Returns the Z coordinate at the given t-value on the curve</summary>
		/// <param name="t">The t-value along the curve to sample</param>
		public float GetPointZ( float t ) {
			float a = p0.z + ( p1.z - p0.z ) * t; // a = lerp( p0, p1, t );
			float b = p1.z + ( p2.z - p1.z ) * t; // b = lerp( p1, p2, t );
			float c = p2.z + ( p3.z - p2.z ) * t; // c = lerp( p2, p3, t );
			float d = a + ( b - a ) * t; // d = lerp( a, b, t );
			float e = b + ( c - b ) * t; // e = lerp( b, c, t );
			return d + ( e - d ) * t; // ret lerp( d, e, t );
		}

		/// <summary>Returns a component of the coordinate at the given t-value on the curve</summary>
		/// <param name="component">Which component of the coordinate to return. 0 is X, 1 is Y, 2 is Z</param>
		/// <param name="t">The t-value along the curve to sample</param>
		public float GetPointComponent( int component, float t ) {
			switch( component ) {
				case 0:  return GetPointX( t );
				case 1:  return GetPointY( t );
				case 2:  return GetPointZ( t );
				default: throw new ArgumentOutOfRangeException( nameof(component), "component has to be either 0, 1 or 2" );
			}
		}
	}

	#endregion

	#region Derivative

	public partial struct BezierCubic2D {
		/// <summary>Returns the derivative at the given t-value on the curve. Loosely analogous to "velocity" of the point along the curve</summary>
		/// <param name="t">The t-value along the curve to sample</param>
		public Vector2 GetDerivative( float t ) {
			float ax = p0.x + ( p1.x - p0.x ) * t; // a = lerp( p0, p1, t );
			float ay = p0.y + ( p1.y - p0.y ) * t;
			float bx = p1.x + ( p2.x - p1.x ) * t; // b = lerp( p1, p2, t );
			float by = p1.y + ( p2.y - p1.y ) * t;
			float cx = p2.x + ( p3.x - p2.x ) * t; // c = lerp( p2, p3, t );
			float cy = p2.y + ( p3.y - p2.y ) * t;
			float dx = ax + ( bx - ax ) * t; // d = lerp( a, b, t );
			float dy = ay + ( by - ay ) * t;
			float ex = bx + ( cx - bx ) * t; // e = lerp( b, c, t );
			float ey = by + ( cy - by ) * t;
			return new Vector2( 3 * ( ex - dx ), 3 * ( ey - dy ) ); // 3*(e - d)
		}
	}

	public partial struct BezierCubic3D {
		/// <summary>Returns the derivative at the given t-value on the curve. Loosely analogous to "velocity" of the point along the curve</summary>
		/// <param name="t">The t-value along the curve to sample</param>
		public Vector3 GetDerivative( float t ) {
			float ax = p0.x + ( p1.x - p0.x ) * t; // a = lerp( p0, p1, t );
			float ay = p0.y + ( p1.y - p0.y ) * t;
			float az = p0.z + ( p1.z - p0.z ) * t;
			float bx = p1.x + ( p2.x - p1.x ) * t; // b = lerp( p1, p2, t );
			float by = p1.y + ( p2.y - p1.y ) * t;
			float bz = p1.z + ( p2.z - p1.z ) * t;
			float cx = p2.x + ( p3.x - p2.x ) * t; // c = lerp( p2, p3, t );
			float cy = p2.y + ( p3.y - p2.y ) * t;
			float cz = p2.z + ( p3.z - p2.z ) * t;
			float dx = ax + ( bx - ax ) * t; // d = lerp( a, b, t );
			float dy = ay + ( by - ay ) * t;
			float dz = az + ( bz - az ) * t;
			float ex = bx + ( cx - bx ) * t; // e = lerp( b, c, t );
			float ey = by + ( cy - by ) * t;
			float ez = bz + ( cz - bz ) * t;
			return new Vector3( 3 * ( ex - dx ), 3 * ( ey - dy ), 3 * ( ez - dz ) ); // 3*(e - d)
		}
	}

	#endregion

	#region Second Derivative

	public partial struct BezierCubic2D {
		/// <summary>Returns the second derivative at the given t-value on the curve. Loosely analogous to "acceleration" of the point along the curve</summary>
		/// <param name="t">The t-value along the curve to sample</param>
		public Vector2 GetSecondDerivative( float t ) { // unrolled code for performance reasons
			float ax = p0.x + ( p1.x - p0.x ) * t; // a = lerp( p0, p1, t );
			float ay = p0.y + ( p1.y - p0.y ) * t;
			float bx = p1.x + ( p2.x - p1.x ) * t; // b = lerp( p1, p2, t );
			float by = p1.y + ( p2.y - p1.y ) * t;
			float cx = p2.x + ( p3.x - p2.x ) * t; // c = lerp( p2, p3, t );
			float cy = p2.y + ( p3.y - p2.y ) * t;
			return new Vector2(
				6 * ( ax - 2 * bx + cx ),
				6 * ( ay - 2 * by + cy )
			);
		}
	}

	public partial struct BezierCubic3D {
		/// <summary>Returns the second derivative at the given t-value on the curve. Loosely analogous to "acceleration" of the point along the curve</summary>
		/// <param name="t">The t-value along the curve to sample</param>
		public Vector3 GetSecondDerivative( float t ) { // unrolled code for performance reasons
			float ax = p0.x + ( p1.x - p0.x ) * t; // a = lerp( p0, p1, t );
			float ay = p0.y + ( p1.y - p0.y ) * t;
			float az = p0.z + ( p1.z - p0.z ) * t;
			float bx = p1.x + ( p2.x - p1.x ) * t; // b = lerp( p1, p2, t );
			float by = p1.y + ( p2.y - p1.y ) * t;
			float bz = p1.z + ( p2.z - p1.z ) * t;
			float cx = p2.x + ( p3.x - p2.x ) * t; // c = lerp( p2, p3, t );
			float cy = p2.y + ( p3.y - p2.y ) * t;
			float cz = p2.z + ( p3.z - p2.z ) * t;
			return new Vector3(
				6 * ( ax - 2 * bx + cx ),
				6 * ( ay - 2 * by + cy ),
				6 * ( az - 2 * bz + cz )
			);
		}
	}

	#endregion

	#region Third Derivative

	public partial struct BezierCubic2D {
		/// <summary>Returns the third derivative at the given t-value on the curve. Loosely analogous to "jerk" (rate of change of acceleration) of the point along the curve</summary>
		/// <param name="t">The t-value along the curve to sample</param>
		public Vector2 GetThirdDerivative( float t ) =>
			new Vector2(
				-6 * p0.x + 18 * p1.x - 18 * p2.x + 6 * p3.x,
				-6 * p0.y + 18 * p1.y - 18 * p2.y + 6 * p3.y
			);
	}

	public partial struct BezierCubic3D {
		/// <summary>Returns the third derivative at the given t-value on the curve. Loosely analogous to "jerk" (rate of change of acceleration) of the point along the curve</summary>
		/// <param name="t">The t-value along the curve to sample</param>
		public Vector3 GetThirdDerivative( float t ) =>
			new Vector3(
				-6 * p0.x + 18 * p1.x - 18 * p2.x + 6 * p3.x,
				-6 * p0.y + 18 * p1.y - 18 * p2.y + 6 * p3.y,
				-6 * p0.z + 18 * p1.z - 18 * p2.z + 6 * p3.z
			);
	}

	#endregion

	#region Tangent

	public partial struct BezierCubic2D {
		/// <summary>Returns the normalized tangent direction at the given t-value on the curve</summary>
		/// <param name="t">The t-value along the curve to sample</param>
		public Vector2 GetTangent( float t ) => GetDerivative( t ).normalized;
	}

	public partial struct BezierCubic3D {
		/// <summary>Returns the normalized tangent direction at the given t-value on the curve</summary>
		/// <param name="t">The t-value along the curve to sample</param>
		public Vector3 GetTangent( float t ) => GetDerivative( t ).normalized;
	}

	#endregion

	// Curvature & Torsion

	#region Curvature

	public partial struct BezierCubic2D {
		/// <summary>Returns the signed curvature at the given t-value on the curve, in radians per distance unit (equivalent to the reciprocal radius of the osculating circle)</summary>
		/// <param name="t">The t-value along the curve to sample</param>
		public float GetCurvature( float t ) {
			( Vector2 vel, Vector2 acc ) = GetFirstTwoDerivatives( t );
			float dMag = vel.magnitude;
			return Determinant( vel, acc ) / ( dMag * dMag * dMag );
		}
	}

	public partial struct BezierCubic3D {
		/// <summary>Returns a pseudovector at the given t-value on the curve, where the magnitude is the curvature in radians per distance unit, and the direction is the axis of curvature</summary>
		/// <param name="t">The t-value along the curve to sample</param>
		public Vector3 GetCurvature( float t ) {
			( Vector3 vel, Vector3 acc ) = GetFirstTwoDerivatives( t );
			float dMag = vel.magnitude;
			return Vector3.Cross( vel, acc ) / ( dMag * dMag * dMag );
		}
	}

	#endregion

	#region Torsion (3D only)

	public partial struct BezierCubic3D {
		/// <summary>Returns the torsion at the given t-value on the curve, in radians per distance unit</summary>
		/// <param name="t">The t-value along the curve to sample</param>
		public float GetTorsion( float t ) {
			( Vector3 vel, Vector3 acc, Vector3 jerk ) = GetAllThreeDerivatives( t );
			Vector3 cVector = Vector3.Cross( vel, acc );
			return Vector3.Dot( cVector, jerk ) / cVector.sqrMagnitude;
		}
	}

	#endregion

	#region Osculating Circle

	public partial struct BezierCubic2D {
		/// <summary>Returns the osculating circle at the given t-value in the curve, if possible. Osculating circles are defined everywhere except on inflection points, where curvature is 0</summary>
		/// <param name="t">The t-value along the curve to sample</param>
		public Circle2D GetOsculatingCircle( float t ) {
			( Vector2 point, Vector2 delta, Vector2 deltaDelta ) = GetPointAndFirstTwoDerivatives( t );
			float dMag = delta.magnitude;
			float curvature = Determinant( delta, deltaDelta ) / ( dMag * dMag * dMag );
			Vector2 tangent = delta.normalized;
			Vector2 normal = tangent.Rotate90CCW();
			float signedRadius = 1f / curvature;
			return new Circle2D( point + normal * signedRadius, Abs( signedRadius ) );
		}
	}

	public partial struct BezierCubic3D {
		/// <summary>Returns the osculating circle at the given t-value in the curve, if possible. Osculating circles are defined everywhere except on inflection points, where curvature is 0</summary>
		/// <param name="t">The t-value along the curve to sample</param>
		public Circle3D GetOsculatingCircle( float t ) {
			( Vector3 point, Vector3 vel, Vector3 acc ) = GetPointAndFirstTwoDerivatives( t );
			float dMag = vel.magnitude;
			Vector3 curvatureVector = Vector3.Cross( vel, acc ) / ( dMag * dMag * dMag );
			( Vector3 axis, float curvature ) = curvatureVector.GetDirAndMagnitude();
			Vector3 normal = Vector3.Cross( vel, Vector3.Cross( acc, vel ) ).normalized;
			float signedRadius = 1f / curvature;
			return new Circle3D( point + normal * signedRadius, axis, Abs( signedRadius ) );
		}
	}

	#endregion

	// Normals, Orientation & Angles

	#region Normal

	public partial struct BezierCubic2D {
		/// <summary>Returns the normal direction at the given t-value on the curve.
		/// This normal will point to the inner arc of the current curvature</summary>
		/// <param name="t">The t-value along the curve to sample</param>
		public Vector2 GetNormal( float t ) => GetTangent( t ).Rotate90CCW();
	}

	public partial struct BezierCubic3D {
		/// <summary>Returns the frenet-serret (curvature-based) normal direction at the given t-value on the curve</summary>
		/// <param name="t">The t-value along the curve to sample</param>
		public Vector3 GetArcNormal( float t ) {
			( Vector3 vel, Vector3 acc ) = GetFirstTwoDerivatives( t );
			return Vector3.Cross( vel, Vector3.Cross( acc, vel ) ).normalized;
		}

		/// <summary>Returns a normal of the curve given a reference up vector and t-value on the curve.
		/// The normal will be perpendicular to both the supplied up vector and the curve</summary>
		/// <param name="t">The t-value along the curve to sample</param>
		/// <param name="up">The reference up vector. The normal will be perpendicular to both the supplied up vector and the curve</param>
		public Vector3 GetNormal( float t, Vector3 up ) {
			Vector3 vel = GetDerivative( t );
			return Vector3.Cross( up, vel ).normalized;
		}
	}

	#endregion

	#region Binormal (3D only)

	public partial struct BezierCubic3D {
		/// <summary>Returns the frenet-serret (curvature-based) binormal direction at the given t-value on the curve</summary>
		/// <param name="t">The t-value along the curve to sample</param>
		public Vector3 GetArcBinormal( float t ) {
			( Vector3 vel, Vector3 acc ) = GetFirstTwoDerivatives( t );
			return Vector3.Cross( vel, acc ).normalized;
		}

		/// <summary>Returns the binormal of the curve given a reference up vector and t-value on the curve.
		/// The binormal will attempt to be as aligned with the reference vector as possible,
		/// while still being perpendicular to the curve</summary>
		/// <param name="t">The t-value along the curve to sample</param>
		/// <param name="up">The reference up vector. The binormal will attempt to be as aligned with the reference vector as possible, while still being perpendicular to the curve</param>
		public Vector3 GetBinormal( float t, Vector3 up ) {
			Vector3 vel = GetDerivative( t );
			Vector3 normal = Vector3.Cross( up, vel ).normalized;
			return Vector3.Cross( vel.normalized, normal );
		}
	}

	#endregion

	#region Angle (2D only)

	public partial struct BezierCubic2D {
		/// <summary>Returns the 2D angle of the direction of the curve at the given point, in radians</summary>
		/// <param name="t">The t-value along the curve to sample</param>
		public float GetAngle( float t ) => DirToAng( GetDerivative( t ) );
	}

	#endregion

	#region Orientation

	public partial struct BezierCubic2D {
		/// <summary>Returns the orientation at the given point t, where the X axis is tangent to the curve</summary>
		/// <param name="t">The t-value along the curve to sample</param>
		public Quaternion GetOrientation( float t ) {
			Vector2 v = GetTangent( t );
			v.x += 1;
			v.Normalize();
			return new Quaternion( 0, 0, v.y, v.x );
		}
	}

	public partial struct BezierCubic3D {
		/// <summary>Returns the orientation at the given point t, where the Z direction is tangent to the curve.
		/// The Y axis will attempt to align with the supplied up vector</summary>
		/// <param name="t">The t-value along the curve to sample</param>
		/// <param name="up">The reference up vector. The Y axis will attempt to be as aligned with this vector as much as possible</param>
		public Quaternion GetOrientation( float t, Vector3 up ) => Quaternion.LookRotation( GetDerivative( t ), up );

		/// <summary>Returns the frenet-serret (curvature-based) orientation of curve at the given point t, where the Z direction is tangent to the curve.
		/// The X axis will point to the inner arc of the current curvature</summary>
		/// <param name="t">The t-value along the curve to sample</param>
		public Quaternion GetArcOrientation( float t ) {
			( Vector3 vel, Vector3 acc ) = GetFirstTwoDerivatives( t );
			Vector3 binormal = Vector3.Cross( vel, acc );
			return Quaternion.LookRotation( vel, binormal );
		}
	}

	#endregion

	#region Pose

	public partial struct BezierCubic2D {
		/// <summary>Returns the position and orientation at the given t-value on the curve</summary>
		/// <param name="t">The t-value along the curve to sample</param>
		public Pose GetPose( float t ) {
			( Vector2 p, Vector2 v ) = GetPointAndTangent( t );
			v.x += 1;
			v.Normalize();
			Quaternion rot = new Quaternion( 0, 0, v.y, v.x );
			return new Pose( p, rot );
		}
	}

	public partial struct BezierCubic3D {
		/// <summary>Returns the position and orientation of curve at the given point t, where the Z direction is tangent to the curve.
		/// The Y axis will attempt to align with the supplied up vector</summary>
		/// <param name="t">The t-value along the curve to sample</param>
		/// <param name="up">The reference up vector. The Y axis will attempt to be as aligned with this vector as much as possible</param>
		public Pose GetPose( float t, Vector3 up ) {
			( Vector2 p, Vector2 v ) = GetPointAndTangent( t );
			return new Pose( p, Quaternion.LookRotation( v, up ) );
		}

		/// <summary>Returns the position and the frenet-serret (curvature-based) orientation of curve at the given point t, where the Z direction is tangent to the curve.
		/// The X axis will point to the inner arc of the current curvature</summary>
		/// <param name="t">The t-value along the curve to sample</param>
		public Pose GetArcPose( float t ) {
			( Vector3 pt, Vector3 vel, Vector3 acc ) = GetPointAndFirstTwoDerivatives( t );
			Vector3 binormal = Vector3.Cross( vel, acc );
			return new Pose( pt, Quaternion.LookRotation( vel, binormal ) );
		}
	}

	#endregion

	#region Matrix

	public partial struct BezierCubic2D {
		/// <summary>Returns the position and orientation at the given t-value on the curve, expressed as a matrix</summary>
		/// <param name="t">The t-value along the curve to sample</param>
		public Matrix4x4 GetMatrix( float t ) {
			( Vector2 P, Vector2 T ) = GetPointAndTangent( t );
			Vector2 N = T.Rotate90CCW();
			return new Matrix4x4(
				new Vector4( T.x, T.y, 0, 0 ),
				new Vector4( N.x, N.y, 0, 0 ),
				new Vector4( 0, 0, 1, 0 ),
				new Vector4( P.x, P.y, 0, 1 )
			);
		}
	}

	public partial struct BezierCubic3D {
		/// <summary>Returns the position and orientation of curve at the given point t, expressed as a matrix, where the Z direction is tangent to the curve.
		/// The Y axis will attempt to align with the supplied up vector</summary>
		/// <param name="t">The t-value along the curve to sample</param>
		/// <param name="up">The reference up vector. The Y axis will attempt to be as aligned with this vector as much as possible</param>
		public Matrix4x4 GetMatrix( float t, Vector3 up ) {
			( Vector3 P, Vector3 T ) = GetPointAndTangent( t );
			Vector3 N = Vector3.Cross( up, T ).normalized; // X axis
			Vector3 B = Vector3.Cross( T, N ); // Y axis
			return new Matrix4x4(
				new Vector4( N.x, N.y, N.z, 0 ),
				new Vector4( B.x, B.y, B.z, 0 ),
				new Vector4( T.x, T.y, T.z, 0 ),
				new Vector4( P.x, P.y, P.z, 1 )
			);
		}

		/// <summary>Returns the position and the frenet-serret (curvature-based) orientation of curve at the given point t, expressed as a matrix, where the Z direction is tangent to the curve.
		/// The X axis will point to the inner arc of the current curvature</summary>
		/// <param name="t">The t-value along the curve to sample</param>
		public Matrix4x4 GetArcMatrix( float t ) {
			( Vector3 P, Vector3 vel, Vector3 acc ) = GetPointAndFirstTwoDerivatives( t );
			Vector3 T = vel.normalized;
			Vector3 B = Vector3.Cross( vel, acc ).normalized;
			Vector3 N = Vector3.Cross( B, T );
			return new Matrix4x4(
				new Vector4( N.x, N.y, N.z, 0 ),
				new Vector4( B.x, B.y, B.z, 0 ),
				new Vector4( T.x, T.y, T.z, 0 ),
				new Vector4( P.x, P.y, P.z, 1 )
			);
		}
	}

	#endregion

	// Whole-curve properties

	#region Splitting

	public partial struct BezierCubic2D {
		/// <summary>Splits this curve at the given t-value, into two curves of the exact same shape</summary>
		/// <param name="t">The t-value along the curve to sample</param>
		public (BezierCubic2D, BezierCubic2D) Split( float t ) {
			Vector2 a = Vector2.Lerp( p0, p1, t );
			Vector2 b = Vector2.Lerp( p1, p2, t );
			Vector2 c = Vector2.Lerp( p2, p3, t );
			Vector2 d = Vector2.Lerp( a, b, t );
			Vector2 e = Vector2.Lerp( b, c, t );
			Vector2 p = Vector2.Lerp( d, e, t );
			return ( new BezierCubic2D( p0, a, d, p ), new BezierCubic2D( p, e, c, p3 ) );
		}
	}

	public partial struct BezierCubic3D {
		/// <inheritdoc cref="BezierCubic2D.Split(float)"/>
		public (BezierCubic3D, BezierCubic3D) Split( float t ) {
			Vector3 a = Vector3.Lerp( p0, p1, t );
			Vector3 b = Vector3.Lerp( p1, p2, t );
			Vector3 c = Vector3.Lerp( p2, p3, t );
			Vector3 d = Vector3.Lerp( a, b, t );
			Vector3 e = Vector3.Lerp( b, c, t );
			Vector3 p = Vector3.Lerp( d, e, t );
			return ( new BezierCubic3D( p0, a, d, p ), new BezierCubic3D( p, e, c, p3 ) );
		}
	}

	#endregion

	#region Bounds

	public partial struct BezierCubic2D {
		/// <summary>Returns the tight axis-aligned bounds of the curve</summary>
		public Rect GetBounds() {
			// first and last points are always included
			Vector2 min = Vector2.Min( p0, p3 );
			Vector2 max = Vector2.Max( p0, p3 );

			void Encapsulate( int axis, float value ) {
				min[axis] = Min( min[axis], value );
				max[axis] = Max( max[axis], value );
			}

			for( int i = 0; i < 2; i++ ) {
				ResultsMax2<float> extrema = GetLocalExtremaPoints( i );
				for( int j = 0; j < extrema.count; j++ )
					Encapsulate( i, extrema[j] );
			}

			return new Rect( min.x, min.y, max.x - min.x, max.y - min.y );
		}
	}

	public partial struct BezierCubic3D {
		/// <summary>Returns the tight axis-aligned bounds of the curve</summary>
		public Bounds GetBounds() {
			// first and last points are always included
			Vector3 min = Vector3.Min( p0, p3 );
			Vector3 max = Vector3.Max( p0, p3 );

			void Encapsulate( int axis, float value ) {
				min[axis] = Min( min[axis], value );
				max[axis] = Max( max[axis], value );
			}

			for( int i = 0; i < 3; i++ ) {
				ResultsMax2<float> extrema = GetLocalExtremaPoints( i );
				for( int j = 0; j < extrema.count; j++ )
					Encapsulate( i, extrema[j] );
			}

			return new Bounds( ( max + min ) * 0.5f, max - min );
		}
	}

	#endregion

	#region Length

	public partial struct BezierCubic2D {
		/// <summary>Returns the approximate length of the curve</summary>
		/// <param name="accuracy">The number of subdivisions to approximate the length with. Higher values are more accurate, but more expensive to calculate</param>
		public float GetLength( int accuracy = 8 ) {
			if( accuracy <= 2 )
				return ( p0 - p3 ).magnitude;

			float totalDist = 0;
			Vector2 prev = p0;
			for( int i = 1; i < accuracy; i++ ) {
				float t = i / ( accuracy - 1f );
				Vector2 p = GetPoint( t );
				float dx = p.x - prev.x;
				float dy = p.y - prev.y;
				totalDist += Mathf.Sqrt( dx * dx + dy * dy );
				prev = p;
			}

			return totalDist / accuracy;
		}
	}

	public partial struct BezierCubic3D {
		/// <summary>Returns the approximate length of the curve</summary>
		/// <param name="accuracy">The number of subdivisions to approximate the length with. Higher values are more accurate, but more expensive to calculate</param>
		public float GetLength( int accuracy = 8 ) {
			if( accuracy <= 2 )
				return ( p0 - p3 ).magnitude;

			float totalDist = 0;
			Vector3 prev = p0;
			for( int i = 1; i < accuracy; i++ ) {
				float t = i / ( accuracy - 1f );
				Vector3 p = GetPoint( t );
				float dx = p.x - prev.x;
				float dy = p.y - prev.y;
				float dz = p.z - prev.z;
				totalDist += Mathf.Sqrt( dx * dx + dy * dy + dz * dz );
				prev = p;
			}

			return totalDist / accuracy;
		}
	}

	#endregion

	// Point projection & Intersection tests

	#region Project Point

	public partial struct BezierCubic2D {
		/// <summary>Returns the (approximate) point on the curve closest to the input point</summary>
		/// <param name="point">The point to project against the curve</param>
		/// <param name="initialSubdivisions">Recommended range: [8-32]. More subdivisions will be more accurate, but more expensive.
		/// This is how many subdivisions to split the curve into, to find candidates for the closest point.
		/// If your curves are complex, you might need to use around 16 subdivisions.
		/// If they are usually very simple, then around 8 subdivisions is likely fine</param>
		/// <param name="refinementIterations">Recommended range: [3-6]. More iterations will be more accurate, but more expensive.
		/// This is how many times to refine the initial guesses, using Newton's method. This converges rapidly, so high numbers are generally not necessary</param>
		public Vector2 ProjectPoint( Vector2 point, int initialSubdivisions = 16, int refinementIterations = 4 ) => ProjectPoint( point, out _, initialSubdivisions, refinementIterations );

		struct PointProjectSample {
			public float t;
			public float distDeltaSq;
			public Vector2 f;
			public Vector2 fp;
		}

		static PointProjectSample[] pointProjectGuesses = { default, default, default };

		/// <summary>Returns the (approximate) point on the curve closest to the input point</summary>
		/// <param name="point">The point to project against the curve</param>
		/// <param name="t">The t-value at the projected point on the curve</param>
		/// <param name="initialSubdivisions">Recommended range: [8-32]. More subdivisions will be more accurate, but more expensive.
		/// This is how many subdivisions to split the curve into, to find candidates for the closest point.
		/// If your curves are complex, you might need to use around 16 subdivisions.
		/// If they are usually very simple, then around 8 subdivisions is likely fine</param>
		/// <param name="refinementIterations">Recommended range: [3-6]. More iterations will be more accurate, but more expensive.
		/// This is how many times to refine the initial guesses, using Newton's method. This converges rapidly, so high numbers are generally not necessary</param>
		public Vector2 ProjectPoint( Vector2 point, out float t, int initialSubdivisions = 16, int refinementIterations = 4 ) {
			// define a bezier relative to the test point
			BezierCubic2D bez = new BezierCubic2D( p0 - point, p1 - point, p2 - point, p3 - point );

			PointProjectSample SampleDistSqDelta( float tSmp ) {
				PointProjectSample s = new PointProjectSample { t = tSmp };
				( s.f, s.fp ) = bez.GetPointAndDerivative( tSmp );
				s.distDeltaSq = Vector2.Dot( s.f, s.fp );
				return s;
			}

			// find initial candidates
			int candidatesFound = 0;
			PointProjectSample prevSmp = SampleDistSqDelta( 0 );

			for( int i = 1; i < initialSubdivisions; i++ ) {
				float ti = i / ( initialSubdivisions - 1f );
				PointProjectSample smp = SampleDistSqDelta( ti );
				if( SignAsInt( smp.distDeltaSq ) != SignAsInt( prevSmp.distDeltaSq ) ) {
					pointProjectGuesses[candidatesFound++] = SampleDistSqDelta( ( prevSmp.t + smp.t ) / 2 );
					if( candidatesFound == 3 ) break; // no more than three possible candidates because of the polynomial degree
				}

				prevSmp = smp;
			}

			// refine each guess w. Newton-Raphson iterations
			void Refine( ref PointProjectSample smp ) {
				Vector2 fpp = bez.GetSecondDerivative( smp.t );
				float tNew = smp.t - Vector2.Dot( smp.f, smp.fp ) / ( Vector2.Dot( smp.f, fpp ) + Vector2.Dot( smp.fp, smp.fp ) );
				smp = SampleDistSqDelta( tNew );
			}

			for( int p = 0; p < candidatesFound; p++ )
				for( int i = 0; i < refinementIterations; i++ )
					Refine( ref pointProjectGuesses[p] );

			// Now find closest. First include the endpoints
			float sqDist0 = bez.p0.sqrMagnitude; // include endpoints
			float sqDist1 = bez.p3.sqrMagnitude;
			bool firstClosest = sqDist0 < sqDist1;
			float tClosest = firstClosest ? 0 : 1;
			Vector2 ptClosest = firstClosest ? p0 : p3;
			float distSqClosest = firstClosest ? sqDist0 : sqDist1;

			// then check internal roots
			for( int i = 0; i < candidatesFound; i++ ) {
				float pSqmag = pointProjectGuesses[i].f.sqrMagnitude;
				if( pSqmag < distSqClosest ) {
					distSqClosest = pSqmag;
					tClosest = pointProjectGuesses[i].t;
					ptClosest = pointProjectGuesses[i].f + point;
				}
			}

			t = tClosest;
			return ptClosest;
		}

	}

	public partial struct BezierCubic3D {

		/// <summary>Returns the (approximate) point on the curve closest to the input point</summary>
		/// <param name="point">The point to project against the curve</param>
		/// <param name="initialSubdivisions">Recommended range: [8-32]. More subdivisions will be more accurate, but more expensive.
		/// This is how many subdivisions to split the curve into, to find candidates for the closest point.
		/// If your curves are complex, you might need to use around 16 subdivisions.
		/// If they are usually very simple, then around 8 subdivisions is likely fine</param>
		/// <param name="refinementIterations">Recommended range: [3-6]. More iterations will be more accurate, but more expensive.
		/// This is how many times to refine the initial guesses, using Newton's method. This converges rapidly, so high numbers are generally not necessary</param>
		public Vector3 ProjectPoint( Vector3 point, int initialSubdivisions = 16, int refinementIterations = 4 ) => ProjectPoint( point, out _, initialSubdivisions, refinementIterations );

		struct PointProjectSample {
			public float t;
			public float distDeltaSq;
			public Vector3 f;
			public Vector3 fp;
		}

		static PointProjectSample[] pointProjectGuesses = { default, default, default };

		/// <summary>Returns the (approximate) point on the curve closest to the input point</summary>
		/// <param name="point">The point to project against the curve</param>
		/// <param name="t">The t-value at the projected point on the curve</param>
		/// <param name="initialSubdivisions">Recommended range: [8-32]. More subdivisions will be more accurate, but more expensive.
		/// This is how many subdivisions to split the curve into, to find candidates for the closest point.
		/// If your curves are complex, you might need to use around 16 subdivisions.
		/// If they are usually very simple, then around 8 subdivisions is likely fine</param>
		/// <param name="refinementIterations">Recommended range: [3-6]. More iterations will be more accurate, but more expensive.
		/// This is how many times to refine the initial guesses, using Newton's method. This converges rapidly, so high numbers are generally not necessary</param>
		public Vector3 ProjectPoint( Vector3 point, out float t, int initialSubdivisions = 16, int refinementIterations = 4 ) {
			// define a bezier relative to the test point
			BezierCubic3D bez = new BezierCubic3D( p0 - point, p1 - point, p2 - point, p3 - point );

			PointProjectSample SampleDistSqDelta( float tSmp ) {
				PointProjectSample s = new PointProjectSample { t = tSmp };
				( s.f, s.fp ) = bez.GetPointAndDerivative( tSmp );
				s.distDeltaSq = Vector3.Dot( s.f, s.fp );
				return s;
			}

			// find initial candidates
			int candidatesFound = 0;
			PointProjectSample prevSmp = SampleDistSqDelta( 0 );

			for( int i = 1; i < initialSubdivisions; i++ ) {
				float ti = i / ( initialSubdivisions - 1f );
				PointProjectSample smp = SampleDistSqDelta( ti );
				if( SignAsInt( smp.distDeltaSq ) != SignAsInt( prevSmp.distDeltaSq ) ) {
					pointProjectGuesses[candidatesFound++] = SampleDistSqDelta( ( prevSmp.t + smp.t ) / 2 );
					if( candidatesFound == 3 ) break; // no more than three possible candidates because of the polynomial degree
				}

				prevSmp = smp;
			}

			// refine each guess w. Newton-Raphson iterations
			void Refine( ref PointProjectSample smp ) {
				Vector3 fpp = bez.GetSecondDerivative( smp.t );
				float tNew = smp.t - Vector3.Dot( smp.f, smp.fp ) / ( Vector3.Dot( smp.f, fpp ) + Vector3.Dot( smp.fp, smp.fp ) );
				smp = SampleDistSqDelta( tNew );
			}

			for( int p = 0; p < candidatesFound; p++ )
				for( int i = 0; i < refinementIterations; i++ )
					Refine( ref pointProjectGuesses[p] );

			// Now find closest. First include the endpoints
			float sqDist0 = bez.p0.sqrMagnitude; // include endpoints
			float sqDist1 = bez.p3.sqrMagnitude;
			bool firstClosest = sqDist0 < sqDist1;
			float tClosest = firstClosest ? 0 : 1;
			Vector3 ptClosest = firstClosest ? p0 : p3;
			float distSqClosest = firstClosest ? sqDist0 : sqDist1;

			// then check internal roots
			for( int i = 0; i < candidatesFound; i++ ) {
				float pSqmag = pointProjectGuesses[i].f.sqrMagnitude;
				if( pSqmag < distSqClosest ) {
					distSqClosest = pSqmag;
					tClosest = pointProjectGuesses[i].t;
					ptClosest = pointProjectGuesses[i].f + point;
				}
			}

			t = tClosest;
			return ptClosest;
		}

	}

	#endregion

	#region Intersection Tests (2D only)

	public partial struct BezierCubic2D {

		// Internal - used by all other intersections
		private ResultsMax3<float> Intersect( Vector2 origin, Vector2 direction, bool rangeLimited = false, float minRayT = float.NaN, float maxRayT = float.NaN ) {
			Vector2 p0rel = this.p0 - origin;
			Vector2 p1rel = this.p1 - origin;
			Vector2 p2rel = this.p2 - origin;
			Vector2 p3rel = this.p3 - origin;
			float y0 = Determinant( p0rel, direction ); // transform bezier point components into the line space y components
			float y1 = Determinant( p1rel, direction );
			float y2 = Determinant( p2rel, direction );
			float y3 = Determinant( p3rel, direction );
			Polynomial polynomY = BezierUtils.GetCubicPolynomial( y0, y1, y2, y3 );
			ResultsMax3<float> roots = polynomY.Roots; // t values of the function


			Polynomial polynomX = default;
			if( rangeLimited ) {
				// if we're range limited, we need to verify position along the ray/line/lineSegment
				// and if we do, we need to be able to go from t -> x coord
				float x0 = Vector2.Dot( p0rel, direction ); // transform bezier point components into the line space x components
				float x1 = Vector2.Dot( p1rel, direction );
				float x2 = Vector2.Dot( p2rel, direction );
				float x3 = Vector2.Dot( p3rel, direction );
				polynomX = BezierUtils.GetCubicPolynomial( x0, x1, x2, x3 );
			}

			float CurveTtoRayT( float t ) => polynomX.Sample( t );

			ResultsMax3<float> returnVals = default;

			for( int i = 0; i < roots.count; i++ ) {
				if( roots[i].Between( 0, 1 ) && ( rangeLimited == false || CurveTtoRayT( roots[i] ).Within( minRayT, maxRayT ) ) )
					returnVals = returnVals.Add( roots[i] );
			}

			return returnVals;
		}

		// Internal - to unpack from curve t values to points
		private ResultsMax3<Vector2> TtoPoints( ResultsMax3<float> tVals ) {
			ResultsMax3<Vector2> pts = default;
			for( int i = 0; i < tVals.count; i++ )
				pts = pts.Add( GetPoint( tVals[i] ) );
			return pts;
		}

		/// <summary>Returns the t-values at which the given line intersects with the curve</summary>
		/// <param name="line">The line to test intersection against</param>
		public ResultsMax3<float> Intersect( Line2D line ) => Intersect( line.origin, line.dir );

		/// <summary>Returns the t-values at which the given ray intersects with the curve</summary>
		/// <param name="ray">The ray to test intersection against</param>
		public ResultsMax3<float> Intersect( Ray2D ray ) => Intersect( ray.origin, ray.dir, rangeLimited: true, 0, float.MaxValue );

		/// <summary>Returns the t-values at which the given line segment intersects with the curve</summary>
		/// <param name="lineSegment">The line segment to test intersection against</param>
		public ResultsMax3<float> Intersect( LineSegment2D lineSegment ) => Intersect( lineSegment.start, lineSegment.end - lineSegment.start, rangeLimited: true, 0, lineSegment.LengthSquared );

		/// <summary>Returns the points at which the given line intersects with the curve</summary>
		/// <param name="line">The line to test intersection against</param>
		public ResultsMax3<Vector2> IntersectionPoints( Line2D line ) => TtoPoints( Intersect( line.origin, line.dir ) );

		/// <summary>Returns the points at which the given ray intersects with the curve</summary>
		/// <param name="ray">The ray to test intersection against</param>
		public ResultsMax3<Vector2> IntersectionPoints( Ray2D ray ) => TtoPoints( Intersect( ray.origin, ray.dir, rangeLimited: true, 0, float.MaxValue ) );

		/// <summary>Returns the points at which the given line segment intersects with the curve</summary>
		/// <param name="lineSegment">The line segment to test intersection against</param>
		public ResultsMax3<Vector2> IntersectionPoints( LineSegment2D lineSegment ) => TtoPoints( Intersect( lineSegment.start, lineSegment.end - lineSegment.start, rangeLimited: true, 0, lineSegment.LengthSquared ) );

		/// <summary>Raycasts and returns whether or not it hit, along with the closest hit point</summary>
		/// <param name="ray">The ray to use when raycasting</param>
		/// <param name="hitPoint">The closest point on the curve the ray hit</param>
		/// <param name="maxDist">The maximum length of the ray</param>
		public bool Raycast( Ray2D ray, out Vector2 hitPoint, float maxDist = float.MaxValue ) => Raycast( ray, out hitPoint, out _, maxDist );

		/// <summary>Raycasts and returns whether or not it hit, along with the closest hit point and the t-value on the curve</summary>
		/// <param name="ray">The ray to use when raycasting</param>
		/// <param name="hitPoint">The closest point on the curve the ray hit</param>
		/// <param name="t">The t-value of the curve at the point the ray hit</param>
		/// <param name="maxDist">The maximum length of the ray</param>
		public bool Raycast( Ray2D ray, out Vector2 hitPoint, out float t, float maxDist = float.MaxValue ) {
			float closestDist = float.MaxValue;
			ResultsMax3<float> tPts = Intersect( ray );
			ResultsMax3<Vector2> pts = TtoPoints( tPts );

			// find closest point
			bool didHit = false;
			hitPoint = default;
			t = default;
			for( int i = 0; i < pts.count; i++ ) {
				Vector2 pt = pts[i];
				float dist = Vector2.Dot( ray.dir, pt - ray.origin );
				if( dist < closestDist && dist <= maxDist ) {
					closestDist = dist;
					hitPoint = pt;
					t = tPts[i];
					didHit = true;
				}
			}

			return didHit;
		}

	}

	#endregion

	// Combo functions to save performance

	#region Point & Tangent combo

	public partial struct BezierCubic2D {
		/// <summary>Returns the point and the tangent direction at the given t-value on the curve. This is more performant than calling GetPoint and GetTangent separately</summary>
		/// <param name="t">The t-value along the curve to sample</param>
		public (Vector2, Vector2) GetPointAndTangent( float t ) { // GetPoint(t) and GetTangent(t) unrolled shared code
			float ax = p0.x + ( p1.x - p0.x ) * t; // a = lerp( p0, p1, t );
			float ay = p0.y + ( p1.y - p0.y ) * t;
			float bx = p1.x + ( p2.x - p1.x ) * t; // b = lerp( p1, p2, t );
			float by = p1.y + ( p2.y - p1.y ) * t;
			float cx = p2.x + ( p3.x - p2.x ) * t; // c = lerp( p2, p3, t );
			float cy = p2.y + ( p3.y - p2.y ) * t;
			float dx = ax + ( bx - ax ) * t; // d = lerp( a, b, t );
			float dy = ay + ( by - ay ) * t;
			float ex = bx + ( cx - bx ) * t; // e = lerp( b, c, t );
			float ey = by + ( cy - by ) * t;
			return (
				new Vector2( dx + ( ex - dx ) * t, dy + ( ey - dy ) * t ), // point
				new Vector2( ex - dx, ey - dy ).normalized // tangent. factor of 3 not needed
			);
		}
	}

	public partial struct BezierCubic3D {
		/// <summary>Returns the point and the tangent direction at the given t-value on the curve. This is more performant than calling GetPoint and GetTangent separately</summary>
		/// <param name="t">The t-value along the curve to sample</param>
		public (Vector3, Vector3) GetPointAndTangent( float t ) { // GetPoint(t) and GetTangent(t) unrolled shared code
			float ax = p0.x + ( p1.x - p0.x ) * t; // a = lerp( p0, p1, t );
			float ay = p0.y + ( p1.y - p0.y ) * t;
			float az = p0.z + ( p1.z - p0.z ) * t;
			float bx = p1.x + ( p2.x - p1.x ) * t; // b = lerp( p1, p2, t );
			float by = p1.y + ( p2.y - p1.y ) * t;
			float bz = p1.z + ( p2.z - p1.z ) * t;
			float cx = p2.x + ( p3.x - p2.x ) * t; // c = lerp( p2, p3, t );
			float cy = p2.y + ( p3.y - p2.y ) * t;
			float cz = p2.z + ( p3.z - p2.z ) * t;
			float dx = ax + ( bx - ax ) * t; // d = lerp( a, b, t );
			float dy = ay + ( by - ay ) * t;
			float dz = az + ( bz - az ) * t;
			float ex = bx + ( cx - bx ) * t; // e = lerp( b, c, t );
			float ey = by + ( cy - by ) * t;
			float ez = bz + ( cz - bz ) * t;
			return (
				new Vector3( dx + ( ex - dx ) * t, dy + ( ey - dy ) * t, dz + ( ez - dz ) * t ), // point
				new Vector3( ex - dx, ey - dy, ez - dz ).normalized // tangent. factor of 3 not needed
			);
		}
	}

	#endregion

	#region Point & Derivative combo

	public partial struct BezierCubic2D {
		/// <summary>Returns the point and the derivative at the given t-value on the curve. This is more performant than calling GetPoint and GetDerivative separately</summary>
		/// <param name="t">The t-value along the curve to sample</param>
		public (Vector2, Vector2) GetPointAndDerivative( float t ) { // GetPoint(t) and GetTangent(t) unrolled shared code
			float ax = p0.x + ( p1.x - p0.x ) * t; // a = lerp( p0, p1, t );
			float ay = p0.y + ( p1.y - p0.y ) * t;
			float bx = p1.x + ( p2.x - p1.x ) * t; // b = lerp( p1, p2, t );
			float by = p1.y + ( p2.y - p1.y ) * t;
			float cx = p2.x + ( p3.x - p2.x ) * t; // c = lerp( p2, p3, t );
			float cy = p2.y + ( p3.y - p2.y ) * t;
			float dx = ax + ( bx - ax ) * t; // d = lerp( a, b, t );
			float dy = ay + ( by - ay ) * t;
			float ex = bx + ( cx - bx ) * t; // e = lerp( b, c, t );
			float ey = by + ( cy - by ) * t;
			return (
				new Vector2( dx + ( ex - dx ) * t, dy + ( ey - dy ) * t ), // point
				new Vector2( 3 * ( ex - dx ), 3 * ( ey - dy ) ) // derivative
			);
		}
	}

	public partial struct BezierCubic3D {
		/// <summary>Returns the point and the derivative at the given t-value on the curve. This is more performant than calling GetPoint and GetDerivative separately</summary>
		/// <param name="t">The t-value along the curve to sample</param>
		public (Vector3, Vector3) GetPointAndDerivative( float t ) { // GetPoint(t) and GetTangent(t) unrolled shared code
			float ax = p0.x + ( p1.x - p0.x ) * t; // a = lerp( p0, p1, t );
			float ay = p0.y + ( p1.y - p0.y ) * t;
			float az = p0.z + ( p1.z - p0.z ) * t;
			float bx = p1.x + ( p2.x - p1.x ) * t; // b = lerp( p1, p2, t );
			float by = p1.y + ( p2.y - p1.y ) * t;
			float bz = p1.z + ( p2.z - p1.z ) * t;
			float cx = p2.x + ( p3.x - p2.x ) * t; // c = lerp( p2, p3, t );
			float cy = p2.y + ( p3.y - p2.y ) * t;
			float cz = p2.z + ( p3.z - p2.z ) * t;
			float dx = ax + ( bx - ax ) * t; // d = lerp( a, b, t );
			float dy = ay + ( by - ay ) * t;
			float dz = az + ( bz - az ) * t;
			float ex = bx + ( cx - bx ) * t; // e = lerp( b, c, t );
			float ey = by + ( cy - by ) * t;
			float ez = bz + ( cz - bz ) * t;
			return (
				new Vector3( dx + ( ex - dx ) * t, dy + ( ey - dy ) * t, dz + ( ez - dz ) * t ), // point
				new Vector3( 3 * ( ex - dx ), 3 * ( ey - dy ), 3 * ( ez - dz ) ) // derivative
			);
		}
	}

	#endregion

	#region Derivative & Second Derivative combo

	public partial struct BezierCubic2D {
		/// <summary>Returns the first two derivatives at the given t-value on the curve</summary>
		/// <param name="t">The t-value along the curve to sample</param>
		public (Vector2, Vector2) GetFirstTwoDerivatives( float t ) { // GetDerivative(t) and GetSecondDerivative(t) unrolled shared code
			float ax = p0.x + ( p1.x - p0.x ) * t; // a = lerp( p0, p1, t );
			float ay = p0.y + ( p1.y - p0.y ) * t;
			float bx = p1.x + ( p2.x - p1.x ) * t; // b = lerp( p1, p2, t );
			float by = p1.y + ( p2.y - p1.y ) * t;
			float cx = p2.x + ( p3.x - p2.x ) * t; // c = lerp( p2, p3, t );
			float cy = p2.y + ( p3.y - p2.y ) * t;
			float dx = ax + ( bx - ax ) * t; // d = lerp( a, b, t );
			float dy = ay + ( by - ay ) * t;
			float ex = bx + ( cx - bx ) * t; // e = lerp( b, c, t );
			float ey = by + ( cy - by ) * t;
			return (
				new Vector2( // first derivative
					3 * ( ex - dx ),
					3 * ( ey - dy )
				),
				new Vector2( // second derivative
					6 * ( ax - 2 * bx + cx ),
					6 * ( ay - 2 * by + cy )
				)
			);
		}
	}

	public partial struct BezierCubic3D {
		/// <summary>Returns the first two derivatives at the given t-value on the curve</summary>
		/// <param name="t">The t-value along the curve to sample</param>
		public (Vector3, Vector3) GetFirstTwoDerivatives( float t ) { // GetDerivative(t) and GetSecondDerivative(t) unrolled shared code
			float ax = p0.x + ( p1.x - p0.x ) * t; // a = lerp( p0, p1, t );
			float ay = p0.y + ( p1.y - p0.y ) * t;
			float az = p0.z + ( p1.z - p0.z ) * t;
			float bx = p1.x + ( p2.x - p1.x ) * t; // b = lerp( p1, p2, t );
			float by = p1.y + ( p2.y - p1.y ) * t;
			float bz = p1.z + ( p2.z - p1.z ) * t;
			float cx = p2.x + ( p3.x - p2.x ) * t; // c = lerp( p2, p3, t );
			float cy = p2.y + ( p3.y - p2.y ) * t;
			float cz = p2.z + ( p3.z - p2.z ) * t;
			float dx = ax + ( bx - ax ) * t; // d = lerp( a, b, t );
			float dy = ay + ( by - ay ) * t;
			float dz = az + ( bz - az ) * t;
			float ex = bx + ( cx - bx ) * t; // e = lerp( b, c, t );
			float ey = by + ( cy - by ) * t;
			float ez = bz + ( cz - bz ) * t;
			return (
				new Vector3( // first derivative
					3 * ( ex - dx ),
					3 * ( ey - dy ),
					3 * ( ez - dz )
				),
				new Vector3( // second derivative
					6 * ( ax - 2 * bx + cx ),
					6 * ( ay - 2 * by + cy ),
					6 * ( az - 2 * bz + cz )
				)
			);
		}
	}

	#endregion

	#region Derivative, Second Derivative & Third derivative combo

	public partial struct BezierCubic2D {
		/// <summary>Returns all three derivatives at the given t-value on the curve</summary>
		/// <param name="t">The t-value along the curve to sample</param>
		public (Vector2, Vector2, Vector2) GetAllThreeDerivatives( float t ) { // GetDerivative(t), GetSecondDerivative(t) and GetThirdDerivative(t) unrolled shared code
			float ax = p0.x + ( p1.x - p0.x ) * t; // a = lerp( p0, p1, t );
			float ay = p0.y + ( p1.y - p0.y ) * t;
			float bx = p1.x + ( p2.x - p1.x ) * t; // b = lerp( p1, p2, t );
			float by = p1.y + ( p2.y - p1.y ) * t;
			float cx = p2.x + ( p3.x - p2.x ) * t; // c = lerp( p2, p3, t );
			float cy = p2.y + ( p3.y - p2.y ) * t;
			float dx = ax + ( bx - ax ) * t; // d = lerp( a, b, t );
			float dy = ay + ( by - ay ) * t;
			float ex = bx + ( cx - bx ) * t; // e = lerp( b, c, t );
			float ey = by + ( cy - by ) * t;
			return (
				new Vector2( // first derivative
					3 * ( ex - dx ),
					3 * ( ey - dy )
				),
				new Vector2( // second derivative
					6 * ( ax - 2 * bx + cx ),
					6 * ( ay - 2 * by + cy )
				),
				GetThirdDerivative( t )
			);
		}
	}

	public partial struct BezierCubic3D {
		/// <summary>Returns all three derivatives at the given t-value on the curve</summary>
		/// <param name="t">The t-value along the curve to sample</param>
		public (Vector3, Vector3, Vector3) GetAllThreeDerivatives( float t ) { // GetDerivative(t), GetSecondDerivative(t) and GetThirdDerivative(t) unrolled shared code
			float ax = p0.x + ( p1.x - p0.x ) * t; // a = lerp( p0, p1, t );
			float ay = p0.y + ( p1.y - p0.y ) * t;
			float az = p0.z + ( p1.z - p0.z ) * t;
			float bx = p1.x + ( p2.x - p1.x ) * t; // b = lerp( p1, p2, t );
			float by = p1.y + ( p2.y - p1.y ) * t;
			float bz = p1.z + ( p2.z - p1.z ) * t;
			float cx = p2.x + ( p3.x - p2.x ) * t; // c = lerp( p2, p3, t );
			float cy = p2.y + ( p3.y - p2.y ) * t;
			float cz = p2.z + ( p3.z - p2.z ) * t;
			float dx = ax + ( bx - ax ) * t; // d = lerp( a, b, t );
			float dy = ay + ( by - ay ) * t;
			float dz = az + ( bz - az ) * t;
			float ex = bx + ( cx - bx ) * t; // e = lerp( b, c, t );
			float ey = by + ( cy - by ) * t;
			float ez = bz + ( cz - bz ) * t;
			return (
				new Vector3( // first derivative
					3 * ( ex - dx ),
					3 * ( ey - dy ),
					3 * ( ez - dz )
				),
				new Vector3( // second derivative
					6 * ( ax - 2 * bx + cx ),
					6 * ( ay - 2 * by + cy ),
					6 * ( az - 2 * bz + cz )
				),
				GetThirdDerivative( t )
			);
		}
	}

	#endregion

	#region Point, Derivative & Second Derivative combo

	public partial struct BezierCubic2D {
		/// <summary>Returns the point and the first two derivatives at the given t-value on the curve. This is faster than calling them separately</summary>
		/// <param name="t">The t-value along the curve to sample</param>
		public (Vector2, Vector2, Vector2) GetPointAndFirstTwoDerivatives( float t ) { // GetPoint(t), GetDerivative(t) and GetSecondDerivative(t) unrolled shared code
			float ax = p0.x + ( p1.x - p0.x ) * t; // a = lerp( p0, p1, t );
			float ay = p0.y + ( p1.y - p0.y ) * t;
			float bx = p1.x + ( p2.x - p1.x ) * t; // b = lerp( p1, p2, t );
			float by = p1.y + ( p2.y - p1.y ) * t;
			float cx = p2.x + ( p3.x - p2.x ) * t; // c = lerp( p2, p3, t );
			float cy = p2.y + ( p3.y - p2.y ) * t;
			float dx = ax + ( bx - ax ) * t; // d = lerp( a, b, t );
			float dy = ay + ( by - ay ) * t;
			float ex = bx + ( cx - bx ) * t; // e = lerp( b, c, t );
			float ey = by + ( cy - by ) * t;
			return (
				new Vector2( // point
					dx + ( ex - dx ) * t,
					dy + ( ey - dy ) * t
				),
				new Vector2( // first derivative
					3 * ( ex - dx ),
					3 * ( ey - dy )
				),
				new Vector2( // second derivative
					6 * ( ax - 2 * bx + cx ),
					6 * ( ay - 2 * by + cy )
				)
			);
		}
	}

	public partial struct BezierCubic3D {
		/// <summary>Returns the point and the first two derivatives at the given t-value on the curve. This is faster than calling them separately</summary>
		/// <param name="t">The t-value along the curve to sample</param>
		public (Vector3, Vector3, Vector3) GetPointAndFirstTwoDerivatives( float t ) { // GetPoint(t), GetDerivative(t) and GetSecondDerivative(t) unrolled shared code
			float ax = p0.x + ( p1.x - p0.x ) * t; // a = lerp( p0, p1, t );
			float ay = p0.y + ( p1.y - p0.y ) * t;
			float az = p0.z + ( p1.z - p0.z ) * t;
			float bx = p1.x + ( p2.x - p1.x ) * t; // b = lerp( p1, p2, t );
			float by = p1.y + ( p2.y - p1.y ) * t;
			float bz = p1.z + ( p2.z - p1.z ) * t;
			float cx = p2.x + ( p3.x - p2.x ) * t; // c = lerp( p2, p3, t );
			float cy = p2.y + ( p3.y - p2.y ) * t;
			float cz = p2.z + ( p3.z - p2.z ) * t;
			float dx = ax + ( bx - ax ) * t; // d = lerp( a, b, t );
			float dy = ay + ( by - ay ) * t;
			float dz = az + ( bz - az ) * t;
			float ex = bx + ( cx - bx ) * t; // e = lerp( b, c, t );
			float ey = by + ( cy - by ) * t;
			float ez = bz + ( cz - bz ) * t;
			return (
				new Vector3( // point
					dx + ( ex - dx ) * t,
					dy + ( ey - dy ) * t,
					dz + ( ez - dz ) * t
				),
				new Vector3( // first derivative
					3 * ( ex - dx ),
					3 * ( ey - dy ),
					3 * ( ez - dz )
				),
				new Vector3( // second derivative
					6 * ( ax - 2 * bx + cx ),
					6 * ( ay - 2 * by + cy ),
					6 * ( az - 2 * bz + cz )
				)
			);
		}
	}

	#endregion

	// Misc esoteric mathy stuff

	#region Polynomial Factors

	public partial struct BezierCubic2D {
		/// <summary>Returns the factors of the derivative polynomials, per-component, in the form at²+bt+c</summary>
		public (Vector2 a, Vector2 b, Vector2 c) GetDerivativeFactors() {
			Polynomial X = BezierUtils.GetCubicPolynomialDerivative( p0.x, p1.x, p2.x, p3.x );
			Polynomial Y = BezierUtils.GetCubicPolynomialDerivative( p0.y, p1.y, p2.y, p3.y );
			return (
				new Vector2( X.fQuadratic, Y.fQuadratic ),
				new Vector2( X.fLinear, Y.fLinear ),
				new Vector2( X.fConstant, Y.fConstant ) );
		}

		/// <summary>Returns the factors of the second derivative polynomials, per-component, in the form at+b</summary>
		public (Vector2 a, Vector2 b) GetSecondDerivativeFactors() {
			Polynomial X = BezierUtils.GetCubicPolynomialSecondDerivative( p0.x, p1.x, p2.x, p3.x );
			Polynomial Y = BezierUtils.GetCubicPolynomialSecondDerivative( p0.y, p1.y, p2.y, p3.y );
			return (
				new Vector2( X.fLinear, Y.fLinear ),
				new Vector2( X.fConstant, Y.fConstant ) );
		}
	}

	public partial struct BezierCubic3D {
		/// <summary>Returns the factors of the derivative polynomials, per-component, in the form at²+bt+c</summary>
		public (Vector3 a, Vector3 b, Vector3 c) GetDerivativeFactors() {
			Polynomial X = BezierUtils.GetCubicPolynomialDerivative( p0.x, p1.x, p2.x, p3.x );
			Polynomial Y = BezierUtils.GetCubicPolynomialDerivative( p0.y, p1.y, p2.y, p3.y );
			Polynomial Z = BezierUtils.GetCubicPolynomialDerivative( p0.z, p1.z, p2.z, p3.z );
			return (
				new Vector3( X.fQuadratic, Y.fQuadratic, Z.fQuadratic ),
				new Vector3( X.fLinear, Y.fLinear, Z.fLinear ),
				new Vector3( X.fConstant, Y.fConstant, Z.fConstant ) );
		}

		/// <summary>Returns the factors of the second derivative polynomials, per-component, in the form at+b</summary>
		public (Vector3 a, Vector3 b) GetSecondDerivativeFactors() {
			Polynomial X = BezierUtils.GetCubicPolynomialSecondDerivative( p0.x, p1.x, p2.x, p3.x );
			Polynomial Y = BezierUtils.GetCubicPolynomialSecondDerivative( p0.y, p1.y, p2.y, p3.y );
			Polynomial Z = BezierUtils.GetCubicPolynomialSecondDerivative( p0.z, p1.z, p2.z, p3.z );
			return (
				new Vector3( X.fLinear, Y.fLinear, Z.fLinear ),
				new Vector3( X.fConstant, Y.fConstant, Z.fConstant ) );
		}
	}

	#endregion

	#region Local Extrema

	public partial struct BezierCubic2D {

		/// <summary>Returns the t values of extrema (local minima/maxima) on a given axis in the 0 &lt; t &lt; 1 range</summary>
		/// <param name="axis">Either 0 (X) or 1 (Y)</param>
		public ResultsMax2<float> GetLocalExtrema( int axis ) {
			if( axis < 0 || axis > 1 )
				throw new ArgumentOutOfRangeException( nameof(axis), "axis has to be either 0 or 1" );
			Polynomial polynom;
			if( axis == 0 ) // a little silly but the vec[] indexers are kinda expensive
				polynom = BezierUtils.GetCubicPolynomialDerivative( p0.x, p1.x, p2.x, p3.x );
			else
				polynom = BezierUtils.GetCubicPolynomialDerivative( p0.y, p1.y, p2.y, p3.y );
			ResultsMax3<float> roots = polynom.Roots;
			ResultsMax2<float> outPts = default;
			for( int i = 0; i < roots.count; i++ ) {
				float t = roots[i];
				if( t.Between( 0, 1 ) )
					outPts = outPts.Add( t );
			}

			return outPts;
		}

		/// <summary>Returns the extrema points (local minima/maxima points) on a given axis in the 0 &lt; t &lt; 1 range</summary>
		/// <param name="axis">Either 0 (X) or 1 (Y)</param>
		public ResultsMax2<float> GetLocalExtremaPoints( int axis ) {
			ResultsMax2<float> t = GetLocalExtrema( axis );
			ResultsMax2<float> pts = default;
			for( int i = 0; i < t.count; i++ )
				pts = pts.Add( GetPointComponent( axis, t[i] ) );
			return pts;
		}
	}

	public partial struct BezierCubic3D {
		/// <summary>Returns the t values of extrema (local minima/maxima) on a given axis in the 0 &lt; t &lt; 1 range</summary>
		/// <param name="axis">Either 0 (X), 1 (Y) or 2 (Z)</param>
		public ResultsMax2<float> GetLocalExtrema( int axis ) {
			if( axis < 0 || axis > 2 )
				throw new ArgumentOutOfRangeException( nameof(axis), "axis has to be either 0, 1 or 2" );
			Polynomial polynom;
			if( axis == 0 ) // a little silly but the vec[] indexers are kinda expensive
				polynom = BezierUtils.GetCubicPolynomialDerivative( p0.x, p1.x, p2.x, p3.x );
			else if( axis == 1 )
				polynom = BezierUtils.GetCubicPolynomialDerivative( p0.y, p1.y, p2.y, p3.y );
			else
				polynom = BezierUtils.GetCubicPolynomialDerivative( p0.z, p1.z, p2.z, p3.z );
			ResultsMax3<float> roots = polynom.Roots;
			ResultsMax2<float> outPts = default;
			for( int i = 0; i < roots.count; i++ ) {
				float t = roots[i];
				if( t.Between( 0, 1 ) )
					outPts = outPts.Add( t );
			}

			return outPts;
		}

		/// <summary>Returns the extrema points (local minima/maxima points) on a given axis in the 0 &lt; t &lt; 1 range</summary>
		/// <param name="axis">Either 0 (X), 1 (Y) or 2 (Z)</param>
		public ResultsMax2<float> GetLocalExtremaPoints( int axis ) {
			ResultsMax2<float> t = GetLocalExtrema( axis );
			ResultsMax2<float> pts = default;
			for( int i = 0; i < t.count; i++ )
				pts = pts.Add( GetPointComponent( axis, t[i] ) );
			return pts;
		}
	}

	#endregion

}