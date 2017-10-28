# eppz! `Geometry`

* 1.0.13

	+ Included `meta` files to fix example scenes

* 1.0.11

	+ Force text asset serialization

* 1.0.1

	+ Test scenes
		+ `11. Polygon triangulation (1)`
		+ `11. Polygon triangulation (2)`
	+ `Polygon.UpdatePointPositionsWithSource()`
		+ Update child transforms

* 1.0.0

	+ Triangulation
		+ Resolved issue with inner triangles
			+ Skip triangles if centroid not contained by polygon
	+ `Source`
		+ `Mesh`
			+ Component to feed `MeshFilter` components
		+ `Polygon` / `Segment`
			+ Added `coordinates` to process `World` / `Local` coordinates

* 0.9.7

	+ Triangulation
		+ Feature ready (`Triangle.Net` beta 4 hooked up)
		+ API is not quiet done yet

* 0.9.5

	+ Namings
		+ Model classes namespaced into `Model`
		+ Source classes namespaced into `Source`
			+ `Components.PolygonSource` is `Source.Polygon`
			+ `Components.SegmentSource` is `Source.Segment`
			+ `pointTransforms` is `points`
		+ Updated controllers in `Scenes/Controllers`

* 0.9.0

	+ Added `TriangleNetAddOns`
	+ Added `UnityEngineAdOns`	

* 0.8.3

	+ Added `ClipperAddOns`

* 0.8.2

	+ `PolygonSource`
		+ Offset now can be manipulated at runtime
			+ Maintain the original polygon, so offset polygon can be recalculated any time

* 0.8.0

	+ Test scenes
		`8. Segment-Segment intersection point`
		`9. Polygon offset`
		`10. Multiple polygon centroid`
	+ `ExtendedPolygonLineRenderer`
		+ Winding and area hooks moved down here

* 0.7.0

	+ Test scenes
		`4. Polygon-Segment containment`
		`5. Polygon-Polygon containment`
		`6. Vertex facing`
		`7. Polygon area, Polygon winding`
	+ `Segment` (with `SegmentLineRenderer`)
		+ Can now draw normals
		+ `normal` and `perpendicular` calculations moved up from `Edge`
	+ `Polygon`
		+ Fixed `area` (so `winding`) calculations

* 0.6.0

	+ Test scenes
		+ `5. Polygon-Polygon containment (1)`
		+ `5. Polygon-Polygon containment (2)`

* 0.5.9

	+ Polygon / segment sources now can be updates on `Update` or `LateUpdate`
	+ Test scenes
		+ Updated update modes

* 0.5.8

	+ Test scenes
		+ `3. Polygon permiter-Point containment (Default)`
		+ `4. Polygon-Segment containment`
	+ Polygon / segment sources use world position (!) instead local
	+ Line renderers use world positions as well (both segment and polygon)
	+ Polygon and segment sources can be updated on `LateUpdate()`

* 0.5.7

	+ Test scene `2. Polygon permiter-Point containment (Precise)`

* 0.5.6

	+ Test scene `1. Polygon-Segment intersection`

* 0.5.5

	+ Only calculated winding direction
		+ Removed option to define polygon winding
		+ Removed distinct `_signedArea`
			+ `area` is always (!) signed
		+ Polygon calculations grouped together 
		+ Renamed to simply `winding`
	+ `Geometry`
		+ `CentroidOfPolygons` use `area` directly (being always signed)

* 0.5.1

	+ Added `Lines` (renderer classes)
	+ Added `Scenes`
		+ Yet with a single scene `0. Polygon-point containment`

* 0.5.0

	+ Added the root `Geometry.cs` static class
	+ Added `Vector2` / `Vector3` extensions
	+ Added `AddOns` (yet disabled)
	+ Added `Components`
	+ Added `Editor` scripts
	+ Added `Model` classes
	+ Added regions, XML-Doc summaries
	+ Update `README.md`
	
* 0.0.2

	+ Added `Clipper` submodule
	+ Added `Triangle.NET` submodule

* 0.0.1

	+ Ignore `.meta` files (no value for script assets)

* 0.0.0

	+ Initial commit