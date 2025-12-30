<script setup>
import { ref, onMounted } from "vue"
import { useRouter } from "vue-router"

const signupModal = ref(null)
const router = useRouter()

const loading = ref(false)
const errorMessage = ref("")

const form = ref({
  firstName: "",
  lastName: "",
  email: "",
  password: ""
})

onMounted(() => {
  signupModal.value.showModal()
})

function close() {
  signupModal.value.close()
  router.push("/login")
}

async function submitSignup() {
  errorMessage.value = ""
  loading.value = true

  try {
    const response = await fetch("http://localhost:5001/api/auth/register", {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify({
        firstName: form.value.firstName,
        lastName: form.value.lastName,
        email: form.value.email,
        password: form.value.password
      })
    })

    const result = await response.json()

    if (!response.ok) {
      errorMessage.value =
        result.errors?.[0] || result.message || "Registration failed"
      return
    }

    signupModal.value.close()
    router.push("/login")

  } catch (error) {
    errorMessage.value = "Cannot connect to server"
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <dialog ref="signupModal" class="modal">
    <div class="modal-box">
      <h3 class="text-lg font-bold mb-4">Sign Up</h3>

      <form @submit.prevent="submitSignup">
        <div class="form-control mb-3">
          <label class="label">First Name</label>
          <input
            v-model="form.firstName"
            type="text"
            class="input input-bordered w-full"
            required
          />
        </div>

        <div class="form-control mb-3">
          <label class="label">Last Name</label>
          <input
            v-model="form.lastName"
            type="text"
            class="input input-bordered w-full"
            required
          />
        </div>

        <div class="form-control mb-3">
          <label class="label">Email</label>
          <input
            v-model="form.email"
            type="email"
            class="input input-bordered w-full"
            required
          />
        </div>

        <div class="form-control mb-3">
          <label class="label">Password</label>
          <input
            v-model="form.password"
            type="password"
            class="input input-bordered w-full"
            required
          />
        </div>

        <!-- Error -->
        <div v-if="errorMessage" class="alert text-white alert-error mb-3">
          {{ errorMessage }}
        </div>

        <div class="modal-action">
          <button
            type="submit"
            class="btn btn-success text-white"
            :disabled="loading"
          >
            {{ loading ? "Signing up..." : "Sign Up" }}
          </button>

          <button type="button" class="btn" @click="close">
            Cancel
          </button>
        </div>
      </form>
    </div>
  </dialog>
</template>
