<template>
	<div class="row" style="margin-top: 1em">
		<div class="col-md-10 offset-md-1">
			<kendo-grid :data-source="localDataSource" :pageable="true" :sortable-mode="'multiple'"
			            :sortable-allow-unsort="true"
			            :sortable-show-indexes="true" :filterable-mode="'row'">
				<kendo-grid-column :field="'ProductID'" :title="'ID'" :width="40"></kendo-grid-column>
				<kendo-grid-column :field="'ProductName'"></kendo-grid-column>
				<kendo-grid-column :field="'UnitPrice'" :title="'Unit Price'" :width="120" :format="'{0:c}'"></kendo-grid-column>
				<kendo-grid-column :field="'UnitsInStock'" :title="'Units In Stock'" :width="120"></kendo-grid-column>
				<kendo-grid-column :field="'Discontinued'" :width="120"></kendo-grid-column>
			</kendo-grid>
		</div>
	</div>
</template>

<script>

	export default {
		name: "OperationsTable",
		data() {
			return {
				localDataSource: [{
					"ProductID": 1,
					"ProductName": "Chai",
					"UnitPrice": 18,
					"UnitsInStock": 39,
					"Discontinued": false,
				},
					{
						"ProductID": 2,
						"ProductName": "Chang",
						"UnitPrice": 17,
						"UnitsInStock": 40,
						"Discontinued": false,
					},
					{
						"ProductID": 3,
						"ProductName": "Aniseed Syrup",
						"UnitPrice": 10,
						"UnitsInStock": 13,
						"Discontinued": false,
					},
					{
						"ProductID": 4,
						"ProductName": "Chef Anton's Cajun Seasoning",
						"UnitPrice": 22,
						"UnitsInStock": 53,
						"Discontinued": false,
					},
					{
						"ProductID": 5,
						"ProductName": "Chef Anton's Gumbo Mix",
						"UnitPrice": 21.35,
						"UnitsInStock": 0,
						"Discontinued": true,
					},
					{
						"ProductID": 6,
						"ProductName": "Grandma's Boysenberry Spread",
						"UnitPrice": 25,
						"UnitsInStock": 120,
						"Discontinued": false,
					}
				]
			}
		}
	}
</script>

<style scoped>

</style>