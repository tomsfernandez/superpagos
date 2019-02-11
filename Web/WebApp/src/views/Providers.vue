<template>
	<div>
		<div style="margin: 1em">
			<div class="row">
				<div class="col-md-10 offset-md-1">
					<div class="card">
						<ProviderCreateForm style="margin: 1em"></ProviderCreateForm>
					</div>
				</div>
			</div>
		</div>
		<div style="margin: 1em">
			<div class="row">
				<div class="col-md-10 offset-md-1">
					<ProviderTable></ProviderTable>
				</div>
			</div>
		</div>
	</div>
</template>

<script>
	import ProviderTable from "../components/ProviderTable";
	import ProviderCreateForm from "../components/ProviderCreateForm";
	export default {
		name: "Providers",
		components: {ProviderCreateForm, ProviderTable},
	}
</script>

<style scoped>

</style>