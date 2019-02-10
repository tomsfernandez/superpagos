<template>
	<div class="alert alert-danger" role="alert" v-show="showErrorMessages" data-cy="error-div" id="errorMessages">
		<ul>
			<li :key="msg" v-for="msg in errors">
				{{msg}}
			</li>
		</ul>
	</div>
</template>

<script>
	export default {
		name: "ErrorMessages",
		props: {
			errors: {
				type: Array,
				default: () => []
			}
		},
		computed: {
			showErrorMessages() {
				return this.errors.length > 0;
			}
		}
	}
</script>

<style scoped>

</style>