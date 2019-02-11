<template>
		<div style="margin: 1em">
			<div class="row">
				<div class="col-md-10 offset-md-1">
					<PaymentMethodTable></PaymentMethodTable>
				</div>
			</div>
		</div>
</template>

<script>
	import PaymentMethodTable from "../components/PaymentMethodTable";

	export default {
		name: "PaymentMethods",
		components: {PaymentMethodTable}
	}
</script>

<style scoped>

</style>