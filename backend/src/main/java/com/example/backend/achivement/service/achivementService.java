package com.example.backend.achivement.service;

import com.example.backend.achivement.mapper.achivementMapper;
import com.example.backend.model.AchivementModel;
import com.example.backend.model.UserModel;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

@Service
public class achivementService {
    @Autowired
    private achivementMapper achivementMapper;

    public AchivementModel getAchivement(AchivementModel achivementModel){
        return achivementMapper.getAchivement(achivementModel);
    }
}
