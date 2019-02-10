<template>
  <div class="home">
    <Nav></Nav>
    <router-view></router-view>
  </div>
</template>

<script>
// @ is an alias to /src
import Nav from '@/components/Nav';

export default {
  name: 'home',
  components: {
    Nav
  }
}
</script>
