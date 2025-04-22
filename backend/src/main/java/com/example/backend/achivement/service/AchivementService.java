package com.example.backend.achivement.service;

import com.example.backend.achivement.mapper.AchivementMapper;
import com.example.backend.model.AchivementModel;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

@Service
public class AchivementService {
    @Autowired
    private AchivementMapper achivementMapper;

    public AchivementModel getAchivement(AchivementModel achivementModel){
        return achivementMapper.getAchivement(achivementModel);
    }
}
