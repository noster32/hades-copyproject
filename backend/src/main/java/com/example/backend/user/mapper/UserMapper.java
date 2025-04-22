package com.example.backend.user.mapper;

import com.example.backend.model.UserModel;
import org.apache.ibatis.annotations.Mapper;

@Mapper
public interface UserMapper {

    public UserModel getUser(UserModel userModel);
}
