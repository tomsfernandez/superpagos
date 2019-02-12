<template>
	<nav class="navbar navbar-expand-lg navbar-light bg-light">
		<router-link :to="{path: '/home'}">
			<a class="navbar-brand">Superpagos</a>
		</router-link>
		<button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
			<span class="navbar-toggler-icon"></span>
		</button>
		<div class="collapse navbar-collapse" id="navbarNav">
			<ul class="navbar-nav">
				<li class="nav-item active">
					<a class="nav-link">
						<router-link :to="{name: 'operations'}">Mis operaciones</router-link>
						<span class="sr-only">(current)</span>
					</a>
				</li>
				<li class="nav-item">
					<a class="nav-link">
						<router-link :to="{name: 'methods'}">Medios de pago</router-link>
					</a>
				</li>
				<li v-if="isAdmin" class="nav-item">
					<a class="nav-link">
						<router-link :to="{name: 'providers'}">Proveedores</router-link>
					</a>
				</li>
				<Logout></Logout>
			</ul>
		</div>
	</nav>
</template>

<script>
	import Logout from "./Logout";
	import {mapState} from "vuex";

	export default {
		name: "Nav",
		components: {Logout},
		computed: mapState(['isAdmin']),
	}
</script>

<style scoped>

</style>