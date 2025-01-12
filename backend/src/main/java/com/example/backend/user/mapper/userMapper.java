package com.example.backend.user.mapper;

import com.example.backend.model.UserModel;
import org.apache.ibatis.annotations.Mapper;

import java.util.ArrayList;

@Mapper
public interface userMapper {

    public UserModel getUser(UserModel userModel);
}
