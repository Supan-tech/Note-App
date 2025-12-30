<script setup>
import { ref, watch, computed } from "vue";

const props = defineProps({
  modelValue: Boolean,       
  mode: {                   
    type: String,
    default: "create"
  },
  note: {
    type: Object,
    default: () => ({
      title: "",
      content: ""
    })
  }
});

const emit = defineEmits(["update:modelValue", "save"]);

const title = ref("");
const content = ref("");

watch(
  () => props.modelValue,
  (open) => {
    if (open) {
      title.value = props.note?.title || "";
      content.value = props.note?.content || "";
    }
  }
);

const modalTitle = computed(() =>
  props.mode === "edit" ? "Edit Note" : "New Note"
);

function close() {
  emit("update:modelValue", false);
}

function submit() {
  emit("save", {
    ...props.note,
    title: title.value,
    content: content.value
  });
  close();
}
</script>

<template>
  <dialog class="modal" :open="modelValue">
    <div class="modal-box">
      <h3 class="font-bold text-lg mb-4">
        {{ modalTitle }}
      </h3>

      <form @submit.prevent="submit" class="space-y-4">
        <!-- Title -->
        <div>
          <label class="label">
            <span class="label-text">Title</span>
          </label>
          <input
            v-model="title"
            type="text"
            class="input input-bordered w-full"
            required
          />
        </div>

        <!-- Content -->
        <div>
          <label class="label">
            <span class="label-text">Content</span>
          </label>
          <textarea
            v-model="content"
            class="textarea textarea-bordered w-full"
            rows="4"
            required
          />
        </div>

        <div class="modal-action">
          <button type="button" class="btn" @click="close">
            Cancel
          </button>
          <button class="btn text-white btn-success">
            Save
          </button>
        </div>
      </form>
    </div>

    <!-- click outside -->
    <form method="dialog" class="modal-backdrop">
      <button @click="close"></button>
    </form>
  </dialog>
</template>
