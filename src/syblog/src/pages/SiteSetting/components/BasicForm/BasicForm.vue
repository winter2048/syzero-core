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
        <el-form-item label="网站标题" prop="title">
          <el-input v-model="ruleForm.title"></el-input>
        </el-form-item>
        <el-form-item label="网站图标" prop="name">
          <input
            class="file"
            name="file"
            type="file"
            accept="image/png, image/gif, image/jpeg"
            @change="handleAvatarSuccess"
            style="display:none"
            ref="upload"
          >
          <img v-if="ruleForm.imageUrl" :src="ruleForm.imageUrl" class="avatar" @click="clickfile">
          <i v-else class="el-icon-plus avatar-uploader-icon" @click="clickfile"></i>
        </el-form-item>
        <el-form-item label="网站关键字" prop="keyword">
          <el-input v-model="ruleForm.keyword"></el-input>
        </el-form-item>
        <el-form-item label="网站描述" prop="desc">
          <el-input type="textarea" v-model="ruleForm.desc"></el-input>
        </el-form-item>
        <el-form-item label="网站开关" prop="delivery">
          <el-switch v-model="ruleForm.delivery"></el-switch>
        </el-form-item>

        <el-form-item>
          <el-button type="primary" @click="submitForm('ruleForm')">立即创建</el-button>
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
        title:"",
        keyword:"",
        desc:"",
        imageUrl: "",
        delivery:true
      },
      rules: {
        title: [
          { required: true, message: "请输入网站名称", trigger: "blur" },
          { min: 3, max: 20, message: "长度在 3 到 20 个字符", trigger: "blur" }
        ]
      }
    };
  },
  methods: {
    submitForm(formName) {
      this.$refs[formName].validate(valid => {
        if (valid) {
          alert("submit!");
          console.log(this.ruleForm)
        } else {
          console.log("error submit!!");
          return false;
        }
      });
    },
    resetForm(formName) {
      this.$refs[formName].resetFields();
    },
    handleAvatarSuccess(e) {
      let file = e.target.files[0];
      if (file) {
        this.ruleForm.imageUrl = URL.createObjectURL(file);
      }
    },
    clickfile() {
      this.$refs.upload.click();
    }
  }
};
</script>

<style>
.basic-form {
}

.avatar-uploader .el-upload {
  border: 1px dashed #d9d9d9;
  border-radius: 6px;
  cursor: pointer;
  position: relative;
  overflow: hidden;
}
.avatar-uploader .el-upload:hover {
  border-color: #409eff;
}
.avatar-uploader-icon {
  font-size: 28px;
  color: #8c939d;
  width: 60px;
  height: 60px;
  line-height: 60px;
  text-align: center;
  border: #8c939d 1px solid;
}
.avatar {
  width: 60px;
  height: 60px;
  display: block;
}
</style>
