<script setup>
import { useRouter } from "vue-router"

const router = useRouter()

function logout() {
  // Remove auth data
  localStorage.removeItem("access_token")
  localStorage.removeItem("user")
  router.push("/login")
}
</script>

<template>
  <div class="navbar bg-blue-400 shadow-sm">
    <div class="flex-1">
      <a class="text-white btn btn-ghost text-xl">Note App</a>
    </div>

    <div class="flex gap-2">
      <div class="dropdown dropdown-end">
        <div tabindex="0" role="button" class="btn btn-ghost btn-circle avatar">
          <div class="w-10 rounded-full">
            <img
              alt="Avatar"
              src="https://img.daisyui.com/images/stock/photo-1534528741775-53994a69daeb.webp"
            />
          </div>
        </div>

        <ul
          tabindex="-1"
          class="menu menu-sm dropdown-content bg-base-100 rounded-box z-1 mt-3 w-52 p-2 shadow"
        >
          <li>
            <a @click="logout">Logout</a>
          </li>
        </ul>
      </div>
    </div>
  </div>
</template>
