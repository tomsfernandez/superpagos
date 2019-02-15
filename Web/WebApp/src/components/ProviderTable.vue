<template>
	<kendo-grid :data-source="providers" ref="grid" :pageable="true" :sortable-mode="'multiple'"
	            :sortable-allow-unsort="true" :resizable="true"
	            :sortable-show-indexes="true" :filterable-mode="'row'">
		<kendo-grid-column :field="'id'" :title="'ID'"></kendo-grid-column>
		<kendo-grid-column :field="'name'" :title="'Nombre'"></kendo-grid-column>
		<kendo-grid-column :field="'company'" :title="'Proveedor'"></kendo-grid-column>
		<kendo-grid-column :field="'code'" :title="'Código'"></kendo-grid-column>
		<kendo-grid-column :field="'paymentEndpoint'" :title="'Endpoint de pago'"></kendo-grid-column>
		<kendo-grid-column :field="'rollbackEndpoint'" :title="'Endpoint de devolución'"></kendo-grid-column>
		<kendo-grid-column :field="'linkEndpoint'" :title="'Endpoint de asociación'"></kendo-grid-column>
		<kendo-grid-column :command="{ text: 'Borrar', click: startProviderDelete }"
		                   :title="'&nbsp;'"
		                   :width="100"></kendo-grid-column>
	</kendo-grid>
</template>

<script>
	import {mapActions, mapState} from "vuex";

	export default {

		name: "ProviderTable",
		computed: mapState(['providers']),
		methods: {
			...mapActions(['getProviders', 'deleteProvider']),
			startProviderDelete(e) {
				e.preventDefault();
				const grid = this.$refs.grid.kendoWidget();
				const dataItem = grid.dataItem((e.currentTarget).closest("tr"));
				this.deleteProvider(dataItem.id);
			}
		},
		mounted() {
			this.getProviders();
		}
	}
</script>

<style scoped>

</style>