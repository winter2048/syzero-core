<template>
  <div class="basic-form">
    <basic-container>
      <el-form
        :model="ruleForm"
        :rules="rules"
        ref="ruleForm"
        label-width="100px"
        class="demo-ruleForm"
      >
        <el-form-item label="APPID" prop="appid">
          <el-input v-model="ruleForm.appid"></el-input>
        </el-form-item>
        <el-form-item label="secret" prop="secret">
          <el-input v-model="ruleForm.secret"></el-input>
        </el-form-item>
        <el-form-item label="签名" prop="signname">
          <el-input v-model="ruleForm.signname"></el-input>
        </el-form-item>
        <el-form-item label="注册模板" prop="tempzc">
          <el-input v-model="ruleForm.tempzc"></el-input>
        </el-form-item>
        <el-form-item label="登录模板" prop="tempdl">
          <el-input v-model="ruleForm.tempdl"></el-input>
        </el-form-item>
        <el-form-item label="找回密码模板" prop="tempzhmm">
          <el-input v-model="ruleForm.tempzhmm"></el-input>
        </el-form-item>
        <el-form-item label="修改密码模板" prop="tempxgmm">
          <el-input v-model="ruleForm.tempxgmm"></el-input>
        </el-form-item>
        <el-form-item>
          <el-button type="primary" @click="submitForm('ruleForm')">立即保存</el-button>
          <el-button @click="resetForm('ruleForm')">重置</el-button>
        </el-form-item>
      </el-form>
    </basic-container>
  </div>
</template>

<script>
import BasicContainer from "@vue-materials/basic-container";

export default {
  components: { BasicContainer },
  name: "BasicForm",

  data() {
    return {
      ruleForm: {
        appid: "",
        secret: "",
        signname: "",
        tempzc: "",
        tempdl: "",
        tempzhmm: "",
        tempxgmm: ""
      },
      rules: {
        
      }
    };
  },
  methods: {
    submitForm(formName) {
      this.$refs[formName].validate(valid => {
        if (valid) {
         
          console.log(this.ruleForm);
          alert("submit!");
        } else {
          console.log("error submit!!");
          return false;
        }
      });
    },
    resetForm(formName) {
      this.$refs[formName].resetFields();
    }
  }
};
</script>

<style>
.basic-form {
}
</style>
