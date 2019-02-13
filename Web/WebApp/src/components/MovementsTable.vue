<template>
	<div class="row" style="margin-top: 1em">
		<div class="col-md-10 offset-md-1">
			<kendo-grid :data-source="operations" :pageable="true" :sortable-mode="'multiple'"
			            :sortable-allow-unsort="true" :resizable="true"
			            :sortable-show-indexes="true" :filterable-mode="'row'">
				<kendo-grid-column :field="'id'" :title="'ID'" :width="40"></kendo-grid-column>
				<kendo-grid-column :field="'operationType'" :title="'Tipo de Operación'"></kendo-grid-column>
				<kendo-grid-column :field="'amount'" :title="'Amount'" :format="'{0:c}'"></kendo-grid-column>
				<kendo-grid-column :field="'account.provider.name'" :title="'Nombre del Proveedor'" :width="120"></kendo-grid-column>
				<kendo-grid-column :field="'isSuccesfull'" :title="'Operación Exitosa'"></kendo-grid-column>
			</kendo-grid>
		</div>
	</div>
</template>

<script>
	import {mapActions, mapState} from "vuex";
	
	export default {
		name: "MovementsTable",
		computed: mapState(['operations']),
		methods: {
			...mapActions(['getOperations'])
		},
		mounted() {
			this.getOperations();
		}
	}
</script>

<style scoped>

</style>