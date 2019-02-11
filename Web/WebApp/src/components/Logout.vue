<template>
	<li class="nav-item">
		<a class="nav-link" href="#" @click="logout">Logout</a>
	</li>
</template>

<script>
	export default {
		name: "Logout",
		methods: {
			async logout() {
				await this.$store.dispatch('logout');
				await this.$router.push('/login');
			}
		}
	}
</script>

<style scoped>

</style>