package com.example.backend.user.service;

import com.example.backend.model.UserModel;
import com.example.backend.user.mapper.userMapper;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.ArrayList;

@Service
public class userService {
    @Autowired
    private userMapper userMapper;

    public UserModel getUser(UserModel userModel){
        return userMapper.getUser(userModel);
    }
}
