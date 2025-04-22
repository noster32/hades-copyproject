package com.example.backend.user.service;

import com.example.backend.model.UserModel;
import com.example.backend.user.mapper.UserMapper;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

@Service
public class UserService {
    @Autowired
    private UserMapper userMapper;

    public UserModel getUser(UserModel userModel){
        return userMapper.getUser(userModel);
    }
}
