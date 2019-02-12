<template>
	<div>
		<div class="col-md-10 offset-md-1">
			<kendo-grid :data-source="methods" ref="grid" :pageable="true" :resizable="true"
			            :sortable-mode="'multiple'" :sortable-allow-unsort="true"
			            :sortable-show-indexes="true" :filterable-mode="'row'">
				<kendo-grid-column :field="'id'" :hidden="true"></kendo-grid-column>
				<kendo-grid-column :field="'name'" :title="'Alias'"></kendo-grid-column>
				<kendo-grid-column :field="'company'" :title="'Proveedor'"></kendo-grid-column>
				<kendo-grid-column :field="'creationTimestamp'" :title="'Fecha de creación'"></kendo-grid-column>
				<kendo-grid-column :command="{ text: 'Borrar', click: startMethodDelete }"
				                   :title="'&nbsp;'"
				                   :width="100"></kendo-grid-column>
			</kendo-grid>
		</div>
	</div>
</template>

<script>
	import {mapActions, mapState} from "vuex";

	export default {
		name: "PaymentMethodTable",
		data() {
			return {

			}
		},
		computed: mapState(['methods']),
		methods: {
			...mapActions(['getMethods', 'deleteMethod']),
			startMethodDelete(e) {
				e.preventDefault();
				const grid = this.$refs.grid.kendoWidget();
				const dataItem = grid.dataItem((e.currentTarget).closest("tr"));
				this.deleteMethod(dataItem.id);
			},
		  openButtonModal(e){
            e.preventDefault();
            const grid = this.$refs.grid.kendoWidget();
            const dataItem = grid.dataItem((e.currentTarget).closest("tr"));
            this.deleteMethod(dataItem.id);
		  }
		},
		mounted() {
			this.getMethods();
		}
	}
</script>

<style scoped>

</style>