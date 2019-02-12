<template>
    <div>
        <div class="col-md-10 offset-md-1">
            <kendo-grid :data-source="buttons" ref="grid" :pageable="true" :resizable="true"
                        :sortable-mode="'multiple'" :sortable-allow-unsort="true"
                        :sortable-show-indexes="true" :filterable-mode="'row'">
                <kendo-grid-column :field="'id'" :hidden="true"></kendo-grid-column>
                <kendo-grid-column :field="'code'" :title="'Código'"></kendo-grid-column>
                <kendo-grid-column :field="'providerName'" :title="'Proveedor'"></kendo-grid-column>
                <kendo-grid-column :field="'amount'" :title="'Monto'"></kendo-grid-column>
                <kendo-grid-column :field="'creationDate'" :title="'Fecha de creación'"></kendo-grid-column>
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
    name: "PaymentButtonTable",
    data() {
      return {}
    },
    computed: mapState(['buttons']),
    methods: {
      ...mapActions(['getButtons', 'deleteButton']),
      startMethodDelete(e) {
        e.preventDefault();
        const grid = this.$refs.grid.kendoWidget();
        const dataItem = grid.dataItem((e.currentTarget).closest("tr"));
        this.deleteButton(dataItem.id);
      }
    },
    mounted() {
      this.getButtons();
    }
  }
</script>

<style scoped>

</style>