<template>
  <d2-container class="page">
    <d2-page-cover title="" sub-title="" href="https://github.com/d2-projects/d2-admin11">
    </d2-page-cover>
  </d2-container>
</template>

<script>
import D2HelpBtn from "./components/d2-help-btn";
import D2Badge from "./components/d2-badge";
export default {
  components: {
    D2HelpBtn,
    D2Badge
  }
};
</script>

<style lang="scss" scoped>
@import "~@/assets/style/public.scss";
.page {
  .page__logo {
    width: 120px;
  }
  .page__btn-group {
    color: $color-text-placehoder;
    font-size: 12px;
    margin-top: -10px;
    margin-bottom: 20px;
    span {
      color: $color-text-sub;
      &:hover {
        color: $color-text-main;
      }
    }
  }
}
</style>
