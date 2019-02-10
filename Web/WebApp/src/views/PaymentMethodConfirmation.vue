<template>
	<div></div>
</template>

<script>
	import * as api from "../api";

	export default {
		name: "PaymentMethodConfirmation",
		computed: {
			payload(){
				return {providerCode: this.$route.query.code, operationTokenFromProvider: this.$route.query.token}
			}
		},
		mounted(){
			console.log(this.payload);
			api.createMethod(this.payload).then(() => this.$router.push("methods"));
		}
	}
</script>

<style>

</style>